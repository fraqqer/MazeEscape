using System.Collections.Generic;
using System.Diagnostics;

namespace Ajax.Objects
{
    public class Entity
    {
        private string name;
        private List<Components.IComponent> componentList = new List<Components.IComponent>();
        private Components.ComponentTypes mask;

        public Entity(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Adds a single component to an entity.
        /// </summary>
        /// <param name="component">The component to be added.</param>
        public void AddComponent(Components.IComponent component)
        {
            Debug.Assert(component != null, "Component cannot be null");

            componentList.Add(component);
            mask |= component.ComponentType;
        }

        public string Name
        {
            get { return name; }
        }

        public Components.ComponentTypes Mask
        {
            get { return mask; }
        }

        public List<Components.IComponent> Components
        {
            get { return componentList; }
        }
    }
}