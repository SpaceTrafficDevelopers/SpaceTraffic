using NLog;
/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/
using SpaceTraffic.Dao;
using SpaceTraffic.Engine;
using SpaceTraffic.Entities;
using SpaceTraffic.Entities.Minigames;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Actions;
using SpaceTraffic.Game.Events;
using SpaceTraffic.Game.Minigame;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SpaceTraffic.GameServer
{
    /// <summary>
    /// Minigame manager class.
    /// </summary>
    public class MinigameManager : IMinigameManager
    {
        /// <summary>
        /// Minigames by start action name. K - start action name, V - StartAction with minigames collection
        /// </summary>
        private Dictionary<string, StartAction> minigamesByStartAction;

        /// <summary>
        /// Activce minigames. K - minigame id, V - minigame
        /// </summary>
        private ConcurrentDictionary<int, IMinigame> activeMinigames = new ConcurrentDictionary<int, IMinigame>();

        /// <summary>
        /// Game server instance.
        /// </summary>
        private IGameServer gameServer;

        /// <summary>
        /// Minigame descriptor dao.
        /// </summary>
        private IMinigameDescriptorDAO minigameDescriptorDAO;

        /// <summary>
        /// Start action dao.
        /// </summary>
        private IStartActionDAO startActionDAO;

        /// <summary>
        /// Lock object for minigame id counter.
        /// </summary>
        private object counterLock = new object();

        /// <summary>
        /// Lock objeckt for minigames by start action name dictionary.
        /// </summary>
        private object minigamesByStartActionLock = new object();

        /// <summary>
        /// Static minigame id counter.
        /// </summary>
        private static int minigameCounter = 0;

        /// <summary>
        /// Rewarder servant.
        /// </summary>
        private Rewarder rewarder;

        /// <summary>
        /// ConditionSolwer servant.
        /// </summary>
        private ConditionSolver coditionSolver;

        /// <summary>
        /// MinigameControls servent.
        /// </summary>
        private MinigameControls minigameControls;
        
        /// <summary>
        /// Logger.
        /// </summary>
        private Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Minigame manager constructor.
        /// </summary>
        /// <param name="gameServer">game server instance</param>
        public MinigameManager(IGameServer gameServer)
        {
            this.gameServer = gameServer;
            this.minigameDescriptorDAO = gameServer.Persistence.GetMinigameDescriptorDAO();
            this.startActionDAO = gameServer.Persistence.GetStartActionDAO();
            this.minigameControls = new MinigameControls(gameServer);
            this.coditionSolver = new ConditionSolver(gameServer);
            this.rewarder = new Rewarder(gameServer);
        }

        /// <summary>
        /// Method for loading minigames from database into minigames by start action dictionary.
        /// </summary>
        private void loadMinigames()
        {
            lock (minigamesByStartActionLock)
            {
                this.minigamesByStartAction = this.startActionDAO.GetStartActionsWithMinigamesDictionary();
            }
        }

        public bool registerMinigame(MinigameDescriptor minigame)
        {
            bool correct = minigameControls.checkMinigameDescriptor(minigame);

            if(!correct)
                return false;

            bool insert = this.minigameDescriptorDAO.InsertMinigame(minigame);

            if (insert)
                loadMinigames();

            return insert;
        }

        public bool deregisterMinigame(int minigameDescriptorId)
        {
            bool remove = this.minigameDescriptorDAO.RemoveMinigameById(minigameDescriptorId);

            if (remove)
                loadMinigames();

            return remove;
        }

        public int createGame(int minigameDescriptorId, bool freeGame)
        {
            MinigameDescriptor descriptor = this.minigameDescriptorDAO.GetMinigameById(minigameDescriptorId);
            IMinigame minigame = null;

            if (descriptor != null)
            {
                if (string.IsNullOrEmpty(descriptor.MinigameClassFullName))
                    minigame = new Minigame(getNewMinigameId(), descriptor, gameServer.Game.currentGameTime.Value, freeGame);
                else
                    minigame = createMinigameByFullName(descriptor, freeGame);

                if (minigame != null)
                {
                    this.activeMinigames[minigame.ID] = minigame;
                    return minigame.ID;
                }
            }

            return -1;
        }

        /// <summary>
        /// Method for create minigame instance by class full name.
        /// </summary>
        /// <param name="descriptor">minigame descriptor</param>
        /// <param name="freeGame">indication, if game has to create as free game</param>
        /// <returns>return minigame instance or null</returns>
        private IMinigame createMinigameByFullName(IMinigameDescriptor descriptor, bool freeGame)
        {
            try
            {
                IMinigame minigame = Activator.CreateInstance(Type.GetType(descriptor.MinigameClassFullName)) as IMinigame;

                if (minigame != null)
                {
                    minigame.ID = getNewMinigameId();
                    minigame.Descriptor = descriptor;
                    minigame.CreateTime = gameServer.Game.currentGameTime.Value;
                    minigame.LastRequestTime = minigame.CreateTime;
                    minigame.State = MinigameState.CREATED;
                    minigame.Players = new Dictionary<int, Player>();
                    minigame.FreeGame = freeGame;

                    return minigame;
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                logger.ErrorException(string.Format("Minigame file with assembly qualified name {0} was not found.",
                    descriptor.MinigameClassFullName) , e);
            }

            return null;
        }

        public Result reward(int minigameId, int[] playerIds)
        {
            Result res = null;

            for (int i = 0; i < playerIds.Length; i++)
            {
                res = this.reward(minigameId, playerIds[i]);

                if (res.State == ResultState.FAILURE)
                    return res;
            }

            return res;
        }

        public Result reward(int minigameId, int playerId)
        {
            IMinigame minigame = getActiveGameById(minigameId);

            if (minigame == null)
                return ResultFactory.minigameNotExistsFailure(minigameId);

            if (minigame.FreeGame)
                return ResultFactory.minigameIsFreeGameFailure();

            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(playerId);

            if (player == null)
                return ResultFactory.playerNotExistsFailure(playerId);

            if (!minigameControls.isPlayerInMinigame(minigame, playerId))
                return ResultFactory.playerNotInGameFailure(playerId);

            if (minigameControls.checkState(minigame, MinigameState.FINISHED))
            {
                rewarder.rewardPlayer(player, minigame.Descriptor);
                return ResultFactory.playerRewardedSuccess();
            }

            return ResultFactory.gameNotFinishedFailure();
        }

        public Result startGame(int minigameId)
        {
            IMinigame minigame = getActiveGameById(minigameId);

            if (minigame == null)
                return ResultFactory.minigameNotExistsFailure(minigameId);

            if (!minigameControls.checkState(minigame, MinigameState.PREPARED))
                return ResultFactory.gameIsNotInStateFailure(MinigameState.PREPARED);

            if (minigameControls.checkNumberOfPlayers(minigame))
            {
                minigame.State = MinigameState.PLAYED;
                startCheckLifeAction(minigameId);

                return ResultFactory.minigamegameStartedSuccess();
            }

            return ResultFactory.insufficientNumberOfPlayersFailure();
        }

        /// <summary>
        /// Method for create MinigameLifeControlAction for minigame.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        private void startCheckLifeAction(int minigameId)
        {
            this.updateLastRequestTime(minigameId);
            
            IGameAction action = new MinigameLifeControlAction();
            action.ActionArgs = new object[] { minigameId };
            action.State = GameActionState.PREPARED;
            
            action.Perform(this.gameServer);
        }

        /// <summary>
        /// Method for setting false (minigame id -1) for each players in game.
        /// </summary>
        /// <param name="minigame">minigame</param>
        private void setFalseToIsPlayingMinigame(IMinigame minigame)
        {
            foreach (int playerId in minigame.Players.Keys)
            {
                this.gameServer.World.ActivePlayers[playerId].MinigameId = -1;
            }
        }

        public Result endGame(int minigameId)
        {
            IMinigame minigame = getActiveGameById(minigameId);

            if (minigame == null)
                return ResultFactory.minigameNotExistsFailure(minigameId);
            
            setFalseToIsPlayingMinigame(minigame);
            minigame.State = MinigameState.FINISHED;

            return ResultFactory.minigameFinishedSuccess();
        }

        public List<int> getMinigameList(string actionName, int playerId)
        {
            Player player;
            ICollection<MinigameDescriptor> minigames = getMinigameDescriptorCollectionByActionName(actionName, playerId, out player);

            return minigames == null ? null : coditionSolver.getMinigames(minigames, player);
        }

        public List<MinigameDescriptor> getMinigameDescriptorListByActionName(string actionName, int playerId)
        {
            Player player;
            ICollection<MinigameDescriptor> minigames = getMinigameDescriptorCollectionByActionName(actionName, playerId, out player);

            return minigames == null ? null : coditionSolver.getMinigameDescriptorList(minigames, player);
        }

        public IMinigameDescriptor getMinigameDescriptorByActionName(string actionName, int playerId)
        {
            Player player;
            ICollection<MinigameDescriptor> minigames = getMinigameDescriptorCollectionByActionName(actionName, playerId, out player);

            return minigames == null ? null : coditionSolver.getMinigameDescriptor(minigames, player);
        }

        public int getMinigame(string actionName, int playerId)
        {
            Player player;
            ICollection<MinigameDescriptor> minigames = getMinigameDescriptorCollectionByActionName(actionName, playerId, out player);

            return minigames == null ? -1 : coditionSolver.getMinigame(minigames, player);
        }

        /// <summary>
        /// Method for getting collection of MinigameDescriptors by action name. If player is playing minigame
        /// method return null.
        /// </summary>
        /// <param name="actionName">action name</param>
        /// <param name="playerId">player id</param>
        /// <param name="player">out instance player</param>
        /// <returns>returns collection of minigame descriptor by action name or null</returns>
        private ICollection<MinigameDescriptor> getMinigameDescriptorCollectionByActionName(string actionName, int playerId, out Player player)
        {
            player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(playerId);

            IGamePlayer gamePlayer = this.gameServer.World.GetPlayer(playerId);

            if (player == null || gamePlayer == null || gamePlayer.IsPlayingMinigame)
                return null;

            ICollection<MinigameDescriptor> minigames = getMinigameCollection(actionName, playerId);

            return minigames;
        }

        /// <summary>
        /// Method for getting minigame collection by start action name.
        /// </summary>
        /// <param name="actionName">start action name</param>
        /// <param name="playerId">player id</param>
        /// <returns>return minigames collection or null</returns>
        private ICollection<MinigameDescriptor> getMinigameCollection(string actionName, int playerId)
        {
            ICollection<MinigameDescriptor> minigames;

            lock (minigamesByStartActionLock)
            {
                try
                {
                    minigames = minigamesByStartAction[actionName].Minigames;
                }
                catch (Exception e)
                {
                    logger.InfoException(string.Format("For start action with {0} name does not exist any Minigame.", actionName), e);
                    return null;
                }
            }

            return minigames;
        }

        public Result addPlayer(int minigameId, int playerId)
        {
            IMinigame minigame = getActiveGameById(minigameId);

            if (minigame == null)
                return ResultFactory.minigameNotExistsFailure(minigameId);

            if (minigameControls.checkNumberOfPlayers(minigame))
                return ResultFactory.sufficientNumberOfPlayersFailure();

            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(playerId);

            if (player == null)
                return ResultFactory.playerNotExistsFailure(playerId);

            if (minigameControls.isPlayerInMinigame(minigame, playerId))
                return ResultFactory.playerIsInGameFailure(playerId);

            if (minigameControls.checkState(minigame, MinigameState.CREATED)
                || minigameControls.checkState(minigame, MinigameState.WAITING_FOR_PLAYERS))
            {
                minigame.Players[player.PlayerId] = player;
                this.gameServer.World.ActivePlayers[playerId].MinigameId = minigameId;

                minigame.State = minigame.Players.Count == minigame.Descriptor.PlayerCount
                    ? MinigameState.PREPARED : MinigameState.WAITING_FOR_PLAYERS;

                return ResultFactory.playerAddedSuccess(playerId, minigameId);
            }

            return ResultFactory.minigameWaitingStartedOrFinishedFailure();
        }

        public Result performAction(int minigameId, string actionName, bool lockAction, params object[] actionArgs)
        {
            IMinigame minigame = getActiveGameById(minigameId);

            if (minigame == null)
                return ResultFactory.minigameNotExistsFailure(minigameId);

            if (!this.minigameControls.checkState(minigame, MinigameState.PLAYED))
                return ResultFactory.gameIsNotInStateFailure(MinigameState.PLAYED);
               
            object returnValue = null;
            try
            {
                returnValue = lockAction ? minigame.performActionWithLock(actionName, actionArgs) 
                    : minigame.performAction(actionName, actionArgs);

                return ResultFactory.actionPerformedSuccess(actionName, returnValue);
            }
            catch (Exception e)
            {
                return ResultFactory.exceptionFailure(e);
            }
        }

        public Result performAction(int minigameId, string actionName, params object[] actionArgs)
        {
            return performAction(minigameId, actionName, false, actionArgs);
        }

        /// <summary>
        /// Method for getting minigame by ID from active games.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <returns>return instance of minigame or null</returns>
        private IMinigame getActiveGameById(int minigameId)
        {
            try
            {
                IMinigame minigame = this.activeMinigames[minigameId];
                return minigame;
            }
            catch (KeyNotFoundException e)
            {
                logger.InfoException(string.Format("Minigame with ID {0} was not found in ActiveMinigames.", minigameId), e);
            }

            return null;
        }

        /// <summary>
        /// Method for getting new (unique) minigame ID.
        /// </summary>
        /// <returns>return new minigame ID</returns>
        private int getNewMinigameId()
        {
            lock (counterLock)
            {
                return ++minigameCounter;
            }
        }

        public List<StartAction> getStartActions()
        {
            return startActionDAO.GetStartActions();
        }

        public bool addRelationshipWithStartActions(string minigameName, string startActionName){
            MinigameDescriptor descriptor = this.minigameDescriptorDAO.GetMinigameByName(minigameName);
            StartAction action = this.startActionDAO.GetStartActionByName(startActionName);

            if (action != null && descriptor != null) { 
                bool result = this.minigameDescriptorDAO.InsertRelationshipWithStartActions(descriptor.MinigameId, action.StartActionID);

                if (result) { 
                    loadMinigames();

                    return true;
                }
            }
            return false;
        }

        public bool removeRelationshipWithStartActions(string minigameName, string startActionName)
        {
            MinigameDescriptor descriptor = this.minigameDescriptorDAO.GetMinigameByName(minigameName);
            StartAction action = this.startActionDAO.GetStartActionByName(startActionName);

            if (action != null && descriptor != null){
                bool result = this.minigameDescriptorDAO.RemoveRelationshipWithStartActions(descriptor.MinigameId, action.StartActionID);

                if (result){
                    loadMinigames();

                    return true;
                }
            }
            return false;
        }

        public Result removeGame(int minigameId)
        {
            IMinigame minigame = getActiveGameById(minigameId);

            if(minigame == null)
                return ResultFactory.minigameNotExistsFailure(minigameId);

            if(minigameControls.checkState(minigame, MinigameState.FINISHED) || minigameControls.checkState(minigame, MinigameState.FAILED)){
                    
                try{
                    IMinigame ignore;
                    this.activeMinigames.TryRemove(minigameId, out ignore);

                    return ResultFactory.minigameRemovecSuccess(minigameId);
                }
                catch (KeyNotFoundException e) {
                    logger.InfoException(string.Format("Minigame with ID {0} was not found in ActiveMinigames.", minigameId), e);
                }

                return ResultFactory.removedGameFailure();
            }
            return ResultFactory.gameNotFinishedFailure();
        }
        
        public bool isPlayerPlaying(int playerId)
        {
            return this.minigameControls.isPlayerPlaying(playerId);
        }

        public int actualPlayingMinigameId(int playerId)
        {
            IGamePlayer player = this.gameServer.World.GetPlayer(playerId);

            if (player != null)
                return player.MinigameId;

            return -1;
        }

        public void updateLastRequestTime(int minigameId)
        {
            IMinigame minigame = getActiveGameById(minigameId);

            if (minigame != null)
                minigame.updateLastRequestTime(this.gameServer.Game.currentGameTime.Value);
        }

        public Result checkMinigameLifeAndUpdateLastRequestTime(int minigameId)
        {
            IMinigame minigame = getActiveGameById(minigameId);

            if (minigame == null)
                return ResultFactory.minigameNotExistsFailure(minigameId, false);

            if (minigame.isAlive(this.gameServer.Game.currentGameTime.Value)){
                    
                minigame.updateLastRequestTime(this.gameServer.Game.currentGameTime.Value);
                return ResultFactory.minigameIsAliveSuccess(minigameId, true);
            }
            else{
                this.endGame(minigameId);
                this.removeGame(minigameId);

                return ResultFactory.minigameNotAliveFailure(minigameId, false);
            }               
        }

        public bool checkMinigameLife(int minigameId)
        {
            IMinigame minigame = getActiveGameById(minigameId);
            
            if (minigame != null)
                return minigame.isAlive(this.gameServer.Game.currentGameTime.Value);
            
            return false;
        }

        public void checkLifeOfAllMinigames(long limitForControl)
        {
            foreach (var item in this.activeMinigames)
            {
                int id = item.Value.ID;
                TimeSpan controlLimit = item.Value.CreateTime - this.gameServer.Game.currentGameTime.Value;
                if (controlLimit.TotalMilliseconds > limitForControl && !this.checkMinigameLife(id))
                {
                    this.endGame(id);
                    this.removeGame(id);
                }
            }
        }
    }
}
