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
    public class SpaceshipCargoFinder : Minigame
    {
        private const int WIDTH = 500;
        private const int HEIGHT = 500;
        private const int CELL_SIZE = 25;
        private const int WIN_SCORE = 30;
        private int maxCellWidth;
        private int maxCellHeight;

        private int score;

        public SpaceshipCargoFinder() : base()
        {
            this.maxCellHeight = HEIGHT/ CELL_SIZE;
            this.maxCellWidth = WIDTH / CELL_SIZE;
            this.score = 0;
        }

        public object addScore(){
            this.score++;

            if (this.score >= WIN_SCORE)
                return true;
            else
                return false;
        }

        public object checkCollision(List<Position> body)
        {
            foreach (Position p in body)
            {
                if (p.X < 0 || p.Y < 0 || p.X >= this.maxCellWidth || p.Y >= this.maxCellHeight)
                    return true;
            }
            
            for (int i = 0; i < body.Count; i++) {
                for(int j = i+1; j < body.Count; j++)
                    if (body[i].X == body[j].X && body[i].Y == body[j].Y)
                        return true;
            }

            return false;
        }
    }

    [DataContract]
    public class Position
    {
        [DataMember]
        public int X { get; set; }

        [DataMember]
        public int Y { get; set; }
    }
}
