using SpaceTraffic.Dao;
using SpaceTraffic.Engine;
using SpaceTraffic.Entities;
using SpaceTraffic.Game.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SpaceTraffic.Game.Planner
{
    /// <summary>
    /// Class is used for serializable, deserializable action and creating all path plan.
    /// </summary>
    public class PlannerConverter
    {
        /// <summary>
        /// Instance of binary formatter for serializable and deserializable actions.
        /// </summary>
        private static BinaryFormatter bf = new BinaryFormatter();

        /// <summary>
        /// Create path plan from path plan entity.
        /// </summary>
        /// <param name="entity">Instance of path plan entity which want to convert.</param>
        /// <returns>Path plan.</returns>
        public IPathPlan createPathPlan(PathPlanEntity entity)
        {
            Spaceship ship = getShip(entity);
            IPathPlan plan = new PathPlan(entity.PlayerId, ship, entity.IsCycled);

            createPlanItem(plan, entity);

            return plan;
        }

        /// <summary>
        /// Get spaceship from path plan entity.
        /// </summary>
        /// <param name="entity">Instance of path plan entity.</param>
        /// <returns>Instance of spaceship.</returns>
        private Spaceship getShip(PathPlanEntity entity)
        {
            SpaceShip shipEntity = 
                GameServer.GameServer.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(entity.SpaceShipId);
            
            Spaceship ship = new Spaceship(entity.SpaceShipId, shipEntity.SpaceShipName);
            ship.MaxSpeed = shipEntity.MaxSpeed;

            return ship;
        }

        /// <summary>
        /// Create list of plan item from path plan.
        /// </summary>
        /// <param name="plan">Path plan.</param>
        /// <param name="entity">Path plan entity.</param>
        private void createPlanItem(IPathPlan plan, PathPlanEntity entity)
        {
            IPlanItemEntityDAO pped = GameServer.GameServer.CurrentInstance.Persistence.GetPlanItemEntityDAO();

            List<PlanItemEntity> items = pped.GetPlanItemsByPathPlanId(entity.PathPlanId);

            if (items != null)
            {
                foreach (PlanItemEntity planItem in items)
                {
                    PlanItem pi = new PlanItem();
                    pi.Place = getPlace(planItem);
                    pi.Actions = createPlanActions(planItem);
                    
                    plan.Add(pi);
                }
            }
        }

        /// <summary>
        /// Get place where you was in plan item entity.
        /// </summary>
        /// <param name="entity">Plan item entity.</param>
        /// <returns>Place representing by NavPoint.</returns>
        private NavPoint getPlace(PlanItemEntity entity)
        {
            NavPoint point = new NavPoint();
            
            if(entity.IsPlanet)
                point.Location = GameServer.GameServer.CurrentInstance.World.Map[entity.SolarSystem].Planets[entity.Index];
            else
                point.Location = GameServer.GameServer.CurrentInstance.World.Map[entity.SolarSystem].WormholeEndpoints[int.Parse(entity.Index)];

            return point;
        }

        /// <summary>
        /// Create list of all plannable actions from plan item entity.
        /// </summary>
        /// <param name="entity">Instance of path plan entity.</param>
        /// <returns>List of plannable actions.</returns>
        private List<IPlannableAction> createPlanActions(PlanItemEntity entity)
        {
            IPlanActionDAO pad = GameServer.GameServer.CurrentInstance.Persistence.GetPlanActionDAO();

            List<PlanAction> planActions = pad.GetPlanActionsByPlanItemId(entity.PlanItemId);

            if(planActions == null)
                return null;

            List<IPlannableAction> list = new List<IPlannableAction>();

            foreach (PlanAction action in planActions)
            {
                IPlannableAction pa = convertPlanActions(action);
                list.Add(pa);
            }

            return list;
        }

        /// <summary>
        /// Convert plan action to instance of plannable action (Deserialization).
        /// </summary>
        /// <param name="action">Instance of plan action.</param>
        /// <returns>Plannable action.</returns>
        public static IPlannableAction convertPlanActions(PlanAction action)
        {
            IPlannableAction pa;

            using (MemoryStream memoryStream = new MemoryStream(action.GameAction))
            {
                pa = bf.Deserialize(memoryStream) as IPlannableAction;
            }

            return pa;
        }

        /// <summary>
        /// Convert plannable action to byte array (Serialization).
        /// </summary>
        /// <param name="action">Plannable action.</param>
        /// <returns>Serializate action.</returns>
        public static byte[] convertPlannableAction(IPlannableAction action)
        {
            byte [] array;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                bf.Serialize(memoryStream, action);
                array = memoryStream.ToArray();
            }

            return array;
        }
    }
}
