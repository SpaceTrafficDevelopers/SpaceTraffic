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
using System.Runtime.Serialization;
using System.Text;

namespace SpaceTraffic.Game.Minigame
{
    /// <summary>
    /// Spaceship cargo finder minigame class.
    /// </summary>
    public class SpaceshipCargoFinder : Minigame
    {
        /// <summary>
        /// Game area width.
        /// </summary>
        private const int WIDTH = 500;

        /// <summary>
        /// Game area height.
        /// </summary>
        private const int HEIGHT = 500;

        /// <summary>
        /// Cell size of game area.
        /// </summary>
        private const int CELL_SIZE = 25;

        /// <summary>
        /// Minimal score to win.
        /// </summary>
        private const int WIN_SCORE = 30;

        /// <summary>
        /// Maximum x coordinate of cell in width.
        /// </summary>
        private int maxCellWidth;
        
        /// <summary>
        /// Maximum y coordinate of cell in height.
        /// </summary>
        private int maxCellHeight;

        /// <summary>
        /// Actual game score.
        /// </summary>
        private int score;

        /// <summary>
        /// Spaceship cargo finder constructor.
        /// </summary>
        public SpaceshipCargoFinder() : base()
        {
            this.maxCellHeight = HEIGHT/ CELL_SIZE;
            this.maxCellWidth = WIDTH / CELL_SIZE;
            this.score = 0;
        }

        /// <summary>
        /// Method for adding score.
        /// </summary>
        /// <returns>return true if player wins otherwise false</returns>
        public bool addScore(){
            this.score++;

            if (this.score == WIN_SCORE)
                return true;
            else
                return false;
        }

        /// <summary>
        ///Method for checking if snake is not out of game area bounds 
        ///and if it is not crossing over itself.
        /// </summary>
        /// <param name="body">snake body</param>
        /// <returns>return true when the collision is detected</returns>
        public bool checkCollision(List<Position> body)
        {
            //check boundaries
            foreach (Position p in body)
            {
                if (p.X < 0 || p.Y < 0 || p.X >= this.maxCellWidth || p.Y >= this.maxCellHeight)
                    return true;
            }
            
            //check crossing
            for (int i = 0; i < body.Count; i++) {
                for(int j = i+1; j < body.Count; j++)
                    if (body[i].X == body[j].X && body[i].Y == body[j].Y)
                        return true;
            }

            return false;
        }

        /// <summary>
        /// Method for getting game info.
        /// </summary>
        /// <returns> return info about game</returns>
        public SpaceshipCargoFinderGameInfo getGameInfo()
        {
            SpaceshipCargoFinderGameInfo info = new SpaceshipCargoFinderGameInfo
            {
                ID = this.ID,
                Name = this.Descriptor.Name,
                Width = WIDTH,
                Height = HEIGHT,
                CellSize = CELL_SIZE,
                WinScore = WIN_SCORE,
                RewardCount = (int)this.Descriptor.RewardAmount,
                SnakeLenght = 5
            };

            return info;
        }
    }

    /// <summary>
    /// Crate for snake part body position.
    /// </summary>
    [DataContract]
    public class Position
    {
        /// <summary>
        /// X coordinates.
        /// </summary>
        [DataMember]
        public int X { get; set; }

        /// <summary>
        /// Y coordinates.
        /// </summary>
        [DataMember]
        public int Y { get; set; }
    }

    /// <summary>
    /// Class with info about spaceship cargo finder game.
    /// </summary>
    [DataContract]
    public class SpaceshipCargoFinderGameInfo
    {
        /// <summary>
        /// Game id.
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Game area width.
        /// </summary>
        [DataMember]
        public int Width { get; set; }

        /// <summary>
        /// Game area height.
        /// </summary>
        [DataMember]
        public int Height { get; set; }

        /// <summary>
        /// Game area cell size.
        /// </summary>
        [DataMember]
        public int CellSize { get; set; }

        /// <summary>
        /// Score to win.
        /// </summary>
        [DataMember]
        public int WinScore { get; set; }

        /// <summary>
        /// Initial snake lenght.
        /// </summary>
        [DataMember]
        public int SnakeLenght { get; set; }
        
        /// <summary>
        /// Reward amount.
        /// </summary>
        [DataMember]
        public int RewardCount { get; set; }
    }
}
