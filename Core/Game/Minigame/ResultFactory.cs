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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Minigame
{
    /// <summary>
    /// Result factory for minigame manager.
    /// </summary>
    public static class ResultFactory
    {
        /// <summary>
        /// Failure messages.
        /// </summary>
        #region failure messages

        private const string MINIGAME_NOT_EXISTS = "Hra s id {0} neexistuje.";

        private const string FREE_GAME = "Hra byla rozehrána jako volná hra.";

        private const string PLAYER_NOT_EXISTS = "Hráč s id {0} neexistuje.";

        private const string PLAYER_IS_NOT_IN_GAME = "Hráč s id {0} ve hře není.";

        private const string PLAYER_IS_IN_GAME = "Hráč s id {0} již ve hře je.";

        private const string GAME_NOT_FINISHED = "Hra nebyla ukončena.";

        private const string GAME_NOT_IN_STATE = "Hra není ve stavu {0}.";

        private const string INSUFFICIENT_NUMBER_OF_PLAYERS = "Ke hře není připojen dostatečný počet hráčů.";

        private const string SUFFICIENT_NUMBER_OF_PLAYERS = "Ve hře je dostatečný počet hráčů.";

        private const string MINIGAME_WAITING_STARTED_OR_FINISHED = "Hra čeká na zahájení nebo již byla rozehrána či ukončena.";

        private const string EXCEPTION_MESSAGE = "Metoda skončila vyjímkou : {0}";

        private const string REMOVED_GAME = "Hra již byla pravděpodobně odstraněna.";

        private const string MINIGAME_NOT_ALIVE = "Minihra s id {0} již \"nežije\" a byla ukončena.";

        #endregion failure messages

        /// <summary>
        /// Success messages.
        /// </summary>
        #region success messages

        private const string PLAYER_REWARDED = "Hráč byl odměněn.";

        private const string MINIGAME_STARTED = "Hra byla úspěšně odstartována.";

        private const string MINIGAME_FINISHED = "Hra byla ukončena.";

        private const string MINIGAME_REMOVED = "Hra s id {0} byla odstraněna.";

        private const string PLAYER_ADDED = "Hráč s id {0} byl úspěšně přidán do hry s id {1}.";

        private const string ACTION_PERFORMED = "Akce {0} byla provedena úspěšně.";

        private const string MINIGAME_IS_ALIVE = "Minihra s id {0} \"žije\".";

        #endregion success messages

        #region failure methods

        /// <summary>
        /// Method for getting minigame not exists result.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <returns>failure result without return value</returns>
        public static Result minigameNotExistsFailure(int minigameId)
        {
            return Result.createFailureResult(string.Format(MINIGAME_NOT_EXISTS, minigameId));
        }

        /// <summary>
        /// Method for getting minigame not exists result.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="returnValue">return value</param>
        /// <returns>failure result with return value</returns>
        public static Result minigameNotExistsFailure(int minigameId, object returnValue)
        {
            return Result.createFailureResult(string.Format(MINIGAME_NOT_EXISTS, minigameId),returnValue);
        }

        /// <summary>
        /// Method for getting minigame is free game result.
        /// </summary>
        /// <returns>failure result without return value</returns>
        public static Result minigameIsFreeGameFailure()
        {
            return Result.createFailureResult(FREE_GAME);
        }

        /// <summary>
        /// Method for getting player not exists result.
        /// </summary>
        /// <param name="playerId">player id</param>
        /// <returns>failure result without return value</returns>
        public static Result playerNotExistsFailure(int playerId)
        {
            return Result.createFailureResult(string.Format(PLAYER_NOT_EXISTS, playerId));
        }

        /// <summary>
        /// Method for getting player is not in game result.
        /// </summary>
        /// <param name="playerId">player id</param>
        /// <returns>failure result without return value</returns>
        public static Result playerNotInGameFailure(int playerId)
        {
            return Result.createFailureResult(string.Format(PLAYER_IS_NOT_IN_GAME, playerId));
        }

        /// <summary>
        /// Method for getting player is in game result.
        /// </summary>
        /// <param name="playerId">player id</param>
        /// <returns>failure result without return value</returns>
        public static Result playerIsInGameFailure(int playerId)
        {
            return Result.createFailureResult(string.Format(PLAYER_IS_IN_GAME, playerId));
        }

        /// <summary>
        /// Method for getting minigame is not finished result.
        /// </summary>
        /// <returns>failure result without return value</returns>
        public static Result gameNotFinishedFailure()
        {
            return Result.createFailureResult(GAME_NOT_FINISHED);
        }
        
        /// <summary>
        /// Method for getting minigame is not in state result.
        /// </summary>
        /// <param name="state">minigame state</param>
        /// <returns>failure result without return value</returns>
        public static Result gameIsNotInStateFailure(MinigameState state)
        {
            return Result.createFailureResult(string.Format(GAME_NOT_IN_STATE, state.ToString()));
        }

        /// <summary>
        /// Method for getting insufficient number of players in game result.
        /// </summary>
        /// <returns>failure result without return value</returns>
        public static Result insufficientNumberOfPlayersFailure()
        {
            return Result.createFailureResult(INSUFFICIENT_NUMBER_OF_PLAYERS);
        }

        /// <summary>
        /// Method for getting sufficient number of players in game result.
        /// </summary>
        /// <returns>failure result without return value</returns>
        public static Result sufficientNumberOfPlayersFailure()
        {
            return Result.createFailureResult(SUFFICIENT_NUMBER_OF_PLAYERS);
        }

        /// <summary>
        /// Method for getting result for minigame which is waiting, has been started or has been finished.
        /// </summary>
        /// <returns>failure result without return value</returns>
        public static Result minigameWaitingStartedOrFinishedFailure()
        {
            return Result.createFailureResult(MINIGAME_WAITING_STARTED_OR_FINISHED);
        }

        /// <summary>
        /// Method for getting excetion reuslt. Exception message is in result message.
        /// </summary>
        /// <param name="e">exception</param>
        /// <returns>failure reuslt without reutrn value</returns>
        public static Result exceptionFailure(Exception e)
        {
            return Result.createFailureResult(string.Format(EXCEPTION_MESSAGE, e.Message));
        }

        /// <summary>
        /// Method for getting removed game result.
        /// </summary>
        /// <returns>failure result without return value</returns>
        public static Result removedGameFailure()
        {
            return Result.createFailureResult(REMOVED_GAME);
        }

        /// <summary>
        /// Method for getting minigame is not alive result.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="returnValue">return value</param>
        /// <returns>failure result with return value</returns>
        public static Result minigameNotAliveFailure(int minigameId, object returnValue)
        {
            return Result.createFailureResult(string.Format(MINIGAME_NOT_ALIVE, minigameId), returnValue);
        }

        #endregion failure methods

        #region success methods

        /// <summary>
        /// Method for getting player has been rewarded result.
        /// </summary>
        /// <returns>success result without return value</returns>
        public static Result playerRewardedSuccess()
        {
            return Result.createSuccessResult(PLAYER_REWARDED);
        }

        /// <summary>
        /// Method for getting minigame has been started result.
        /// </summary>
        /// <returns>success result without return value</returns>
        public static Result minigamegameStartedSuccess()
        {
            return Result.createSuccessResult(MINIGAME_STARTED);
        }

        /// <summary>
        /// Method for getting minigame has been finished result.
        /// </summary>
        /// <returns>success result without return value</returns>
        public static Result minigameFinishedSuccess()
        {
            return Result.createSuccessResult(MINIGAME_FINISHED);
        }

        /// <summary>
        /// Method for getting player has been added into minigame result.
        /// </summary>
        /// /// <param name="playerId">player id</param>
        /// <param name="minigameId">minigame id</param>
        /// <returns>success result without return value</returns>
        public static Result playerAddedSuccess(int playerId, int minigameId)
        {
            return Result.createSuccessResult(string.Format(PLAYER_ADDED,playerId, minigameId));
        }

        /// <summary>
        /// Method for getting action has been performed result.
        /// </summary>
        /// /// <param name="actionName">action name</param>
        /// <param name="returnValue">return value</param>
        /// <returns>success result with return value</returns>
        public static Result actionPerformedSuccess(string actionName, object returnValue)
        {
            return Result.createSuccessResult(string.Format(ACTION_PERFORMED, actionName), returnValue);
        }

        /// <summary>
        /// Method for getting minigame has been removed result.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <returns>success result without return value</returns>
        public static Result minigameRemovecSuccess(int minigameId)
        {
            return Result.createSuccessResult(string.Format(MINIGAME_REMOVED, minigameId));
        }

        /// <summary>
        /// Method for getting minigame is alived result.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="returnValue">return value</param>
        /// <returns>success result with return value</returns>
        public static Result minigameIsAliveSuccess(int minigameId, object returnValue)
        {
            return Result.createSuccessResult(string.Format(MINIGAME_IS_ALIVE, minigameId), returnValue);
        }

        #endregion success methods
    }
}
