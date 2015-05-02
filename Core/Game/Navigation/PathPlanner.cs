using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game.Geometry;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Game.Navigation
{
    public static class PathPlanner
    {
        const double DEFAULTBLOCKZONE = 35.0;

        /// <summary>
        /// Solves times of arrival and trajectories for given path.
        /// Data are stored in given instance.
        /// </summary>
        /// <param name="path">The path instance to be solved.</param>
        /// <param name="sh">The space ship.</param>
        /// <param name="startTime">The start time of path.</param>
        public static void SolvePath(NavPath path, SpaceShip sh, double startTime)
        {
            double time = 0.0;
            double eps = (1 / sh.MaxSpeed) > 0.01 ? (1 / sh.MaxSpeed) : 0.01;
            double half = 0.5;

            Point2d startPoint;
            Point2d endPoint = getPosition(path[0], startTime + time);
            Point2d intervalStartPoint;
            Point2d intervalMiddlePoint;
            Point2d intervalEndPoint;

            for (int i = 0; i < path.Count; i++)
            {
                path[i].TimeOfArrival = secondsToDateTime(startTime + time);

                // last point
                if (i + 2 > path.Count) break;

                startPoint = getPosition(path[i], startTime + time);

                // from wormhole to wormhole between different star systems
                if ((path[i].Location is WormholeEndpoint) && path[i + 1].Location is WormholeEndpoint && !path[i].Location.StarSystem.Equals(path[i + 1].Location.StarSystem))
                {
                    path[i].TrajectoryToDest = new Stacionary(startPoint.X, startPoint.Y);
                    continue;
                }
                // next is planet or wormhole
                else
                {
                    // bisection
                    intervalStartPoint = getPosition(path[i + 1], startTime + time);
                    intervalEndPoint = getPosition(path[i + 1], startTime + time);
                    OrbitDefinition orbit = (OrbitDefinition)path[i + 1].Location.Trajectory;
                    double period = orbit.PeriodInSec;
                    double middlePointDiffTime = period * half;
                    double timeStartToIntervalMiddle = 0.0;
                    double timePlanetToIntervalMiddle = middlePointDiffTime;

                    intervalMiddlePoint = getPosition(path[i + 1], time + startTime + timePlanetToIntervalMiddle);

                    while (true)
                    {
                        timeStartToIntervalMiddle = getLine(startPoint, intervalMiddlePoint) / sh.MaxSpeed;
                        double diff = timeStartToIntervalMiddle - timePlanetToIntervalMiddle;
                        if (Math.Abs(timeStartToIntervalMiddle - timePlanetToIntervalMiddle) <= eps)
                        {
                            time += (timeStartToIntervalMiddle + timePlanetToIntervalMiddle) * half;
                            endPoint = getPosition(path[i + 1], startTime + time);
                            break;
                        }
                        else
                        {
                            middlePointDiffTime *= half;
                            if (timeStartToIntervalMiddle < timePlanetToIntervalMiddle)
                            {
                                intervalEndPoint = intervalMiddlePoint;
                                timePlanetToIntervalMiddle -= middlePointDiffTime;
                            }
                            else
                            {
                                intervalStartPoint = intervalMiddlePoint;
                                timePlanetToIntervalMiddle += middlePointDiffTime;
                            }

                            intervalMiddlePoint = getPosition(path[i + 1], time + startTime + timePlanetToIntervalMiddle);
                        }
                    } // endwhile(true)
                }

                LinearTrajectory finalTrajectory = new LinearTrajectory(startPoint, endPoint);
                double blockZone = DEFAULTBLOCKZONE;
                if (path[i].Location.StarSystem.Star.MinimumApproachDistance == 0)
                {
                    blockZone = path[i].Location.StarSystem.Star.MinimumApproachDistance;
                }
                if (LineIntersectBlockZone(finalTrajectory, blockZone))
                {
                    finalTrajectory = new LensTrajectory(startPoint, endPoint, blockZone);
                }
                path[i].TrajectoryToDest = finalTrajectory;
            }
        }

        /// <summary>
        /// Converts seconds to date time.
        /// </summary>
        /// <param name="seconds">Converted seconds.</param>	
        /// <returns>Returns converted date time from seconds.</returns>
        private static DateTime secondsToDateTime(double seconds)
        {
            return new DateTime().Add(TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// Calculates position of a navigation point.
        /// </summary>
        /// <param name="point">Navigation point.</param>	
        /// <param name="time">Current time.</param>	
        /// <returns>Returns position of navigation point.</returns>
        private static Point2d getPosition(NavPoint point, double time)
        {
            return point.Location.Trajectory.CalculatePosition(time);
        }

        /// <summary>
        /// Calculates length of a linear trajectory.
        /// </summary>
        /// <param name="start">Start point of a linear trajectory.</param>	
        /// <param name="end">End point of a linear trajectory.</param>	
        /// <returns>Returns length of a trajectory.</returns>
        private static double getLine(Point2d start, Point2d end)
        {
            return Math.Sqrt((start.X - end.X) * (start.X - end.X) + (start.Y - end.Y) * (start.Y - end.Y));
        }
        /// <summary>	
        /// Checks if a linear trajectory passes through a block zone.	
        /// </summary>
        /// <param name="lineToTarget">Linear trajectory to target point.</param>	
        /// <param name="blockZone">Block zone around the Star.</param>
        /// <returns>Returns true if a linear trajectory passes through a block zone, returns false if not.</returns>	
        private static bool LineIntersectBlockZone(LinearTrajectory lineToTarget, double blockZone)
        {
            double parameterK = (lineToTarget.StartPoint.Y - lineToTarget.EndPoint.Y) / (lineToTarget.StartPoint.X - lineToTarget.EndPoint.X);
            double parameterQ = lineToTarget.StartPoint.Y - parameterK * lineToTarget.StartPoint.X;
            double discriminant = blockZone * blockZone * (1 + parameterK * parameterK) - parameterQ * parameterQ;
            if (discriminant > 0) return true;
            return false;
        }
    }
}
