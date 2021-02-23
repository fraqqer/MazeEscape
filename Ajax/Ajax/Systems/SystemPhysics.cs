using System.Collections.Generic;
using OpenTK;
using Ajax.Objects;
using Ajax.Components;
using Ajax.Systems;

namespace Ajax.Systems
{
    public class SystemPhysics : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY);

        public SystemPhysics() { }

        public string Name
        {
            get { return "SystemPhysics"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentPosition)positionComponent).Position;

                IComponent velocityComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });
                Vector3 velocity = ((ComponentVelocity)velocityComponent).Velocity;

                Motion(ref positionComponent, ref velocityComponent);
            }
        }

        public void Motion(ref IComponent positionComponent, ref IComponent velocityComponent)
        {
            ((ComponentPosition)positionComponent).Position += ((ComponentVelocity)velocityComponent).Velocity * ((ComponentVelocity)velocityComponent).DeltaTime;
        }
    }
}
