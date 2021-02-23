using System.Collections.Generic;
using System.Diagnostics;
using Ajax.Objects;
using Ajax.Components;

namespace Ajax.Managers
{
    public class EntityManager
    {
        List<Entity> entityList;

        public EntityManager()
        {
            entityList = new List<Entity>();
        }

        public void AddEntity(ref Entity entity)
        {
            Entity result = FindEntity(entity.Name);
            Debug.Assert(result == null, "Entity '" + entity.Name + "' already exists");
            entityList.Add(entity);
        }

        public void CloseComponents()
        {
            foreach (Entity entity in entityList)
            {
                foreach (IComponent component in entity.Components)
                {
                    component.Close();
                }
            }
        }

        private Entity FindEntity(string name)
        {
            return entityList.Find(delegate(Entity e)
            {
                return e.Name == name;
            }
            );
        }

        public List<Entity> Entities()
        {
            return entityList;
        }
    }
}
