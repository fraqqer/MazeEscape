using Ajax.Objects;
using Ajax.Systems;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ajax.Managers
{
    public class SystemManager
    {
        List<ISystem> systemList = new List<ISystem>();

        public SystemManager()
        {
        }

        public void ActionSystems(ref EntityManager entityManager)
        {
            List<Entity> entityList = entityManager.Entities();
            foreach(ISystem system in systemList)
            {
                foreach(Entity entity in entityList)
                {
                    system.OnAction(entity);
                }
            }
        }

        public void AddSystem(ref ISystem system)
        {
            ISystem result = FindSystem(system.Name);
            Debug.Assert(result == null, "System '" + system.Name + "' already exists");
            systemList.Add(system);
        }

        private ISystem FindSystem(string name)
        {
            return systemList.Find(delegate(ISystem system)
            {
                return system.Name == name;
            }
            );
        }
    }
}
