using SpaceTraffic.Engine;
using SpaceTraffic.Game.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Planner
{
    /// <summary>
    /// Interface for work with path plan.
    /// </summary>
    public interface IPathPlan : IList<PlanItem>
    {
        /// <summary>
        /// Identification number of player
        /// </summary>
        int PlayerID { get; set; }

        /// <summary>
        /// Instance of spaceship
        /// </summary>
        Spaceship ship { get; set; }

        /// <summary>
        /// Value if plan is cycled or not. 
        /// </summary>
        bool IsCycled { get; set; }

        /// <summary>
        /// Get path from list of plan items
        /// </summary>
        /// <returns>Nav path for planning.</returns>
        NavPath getNavPath();

        /// <summary>
        /// Get path between source plan item and destination plan item.
        /// </summary>
        /// <param name="source">Source plan item.</param>
        /// <param name="dest">Destination plan item.</param>
        /// <returns>Nav Path for planning.</returns>
        NavPath getPathBetweenTwoItems(PlanItem source, PlanItem dest);

        /// <summary>
        /// Method solve path by the start time.
        /// </summary>
        /// <param name="startTime">start time of path plan.</param>
        void SolvePath(double startTime);

        /// <summary>
        /// Get list of event from path.
        /// </summary>
        /// <returns>List of events.</returns>
        List<IGameEvent> getEventsFromPath();

        /// <summary>
        /// Plan all events from follow-up item.
        /// </summary>
        /// <param name="item">Follow-up item.</param>
        /// <param name="gameServer">Instance of game server.</param>
        void planEventsForNextItem(PlanItem item, IGameServer gameServer);

        /// <summary>
        /// Plan first path item.
        /// </summary>
        /// <param name="gameServer">Instance of game server.</param>
        void PlanFirstItem(IGameServer gameServer);

        /// <summary>
        /// Plan spaceship flight between depart point and destination point.
        /// </summary>
        /// <param name="depart">Depart plan item.</param>
        /// <param name="dest">Destination plan item.</param>
        /// <param name="gameServer">Instance of game server.</param>
        /// <param name="ship">Instance of spaceship.</param>
        /// <returns></returns>
        bool PlanFlightBetweenPoints(PlanItem depart, PlanItem dest, IGameServer gameServer, Spaceship ship);


    }
}
