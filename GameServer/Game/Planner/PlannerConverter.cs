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
    public class PlannerConverter
    {
        private static BinaryFormatter bf = new BinaryFormatter();

        public IPathPlan createPathPlan(PathPlanEntity entity)
        {
            Spaceship ship = getShip(entity);
            IPathPlan plan = new PathPlan(entity.PlayerId, ship, entity.IsCycled);

            createPlanItem(plan, entity);

            return plan;
        }

        private Spaceship getShip(PathPlanEntity entity)
        {
            SpaceShip shipEntity = 
                GameServer.GameServer.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(entity.SpaceShipId);
            
            Spaceship ship = new Spaceship(entity.SpaceShipId, shipEntity.SpaceShipName);
            ship.MaxSpeed = shipEntity.MaxSpeed;

            return ship;
        }

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


        private NavPoint getPlace(PlanItemEntity entity)
        {
            NavPoint point = new NavPoint();
            
            if(entity.IsPlanet)
                point.Location = GameServer.GameServer.CurrentInstance.World.Map[entity.SolarSystem].Planets[entity.Index];
            else
                point.Location = GameServer.GameServer.CurrentInstance.World.Map[entity.SolarSystem].WormholeEndpoints[int.Parse(entity.Index)];

            return point;
        }

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

        public static IPlannableAction convertPlanActions(PlanAction action)
        {
            IPlannableAction pa;

            using (MemoryStream memoryStream = new MemoryStream(action.GameAction))
            {
                pa = bf.Deserialize(memoryStream) as IPlannableAction;
            }

            return pa;
        }

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
