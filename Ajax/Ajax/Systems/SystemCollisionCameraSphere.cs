using System.Collections.Generic;
using Ajax;
using Ajax.Components;
using Ajax.Managers;
using Ajax.Objects;
using Ajax.Systems;

namespace Ajax.Systems
{
    public class SystemCollisionCameraSphere : ISystem
    {
        private CollisionManager collisionManager;
        private Camera camera;

        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE);

        public SystemCollisionCameraSphere(CollisionManager collisionManager, Camera camera)
        {
            this.collisionManager = collisionManager;
            this.camera = camera;
        }

        public string Name
        {
            get { return "SystemCollisionCameraSphere"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent collComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE;
                });
                ComponentCollisionSphere collision = (ComponentCollisionSphere)collComponent;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                ComponentPosition position = (ComponentPosition)positionComponent;

                Collision(ref entity, ref position, ref collision);
            }
        }

        public void Collision(ref Entity entity, ref ComponentPosition position, ref ComponentCollisionSphere coll)
        {
            if ((position.Position - camera.cameraPosition).Length < coll.Radius + camera.Radius)
            {
                collisionManager.CollisionBetweenCamera(ref entity, COLLISIONTYPE.SPHERE_SPHERE);
            }
        }
    }
}
