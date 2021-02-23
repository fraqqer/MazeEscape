using System.Collections.Generic;
using OpenTK;
using Ajax;
using Ajax.Components;
using Ajax.Objects;
using Ajax.Managers;
using Ajax.Systems;

namespace Ajax.Systems
{
    public class SystemCollisionCameraLine : ISystem
    {
        private CollisionManager collisionManager;
        private Camera camera;

        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_LINE);

        public SystemCollisionCameraLine(CollisionManager collisionManager, Camera camera)
        {
            this.collisionManager = collisionManager;
            this.camera = camera;
        }

        public string Name
        {
            get { return "SystemCollisionCameraLine"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent collComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_LINE;
                });
                ComponentCollisionLine collision = (ComponentCollisionLine)collComponent;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                ComponentPosition position = (ComponentPosition)positionComponent;

                Collision(ref entity, ref position, ref collision);
            }
        }

        public void Collision(ref Entity entity, ref ComponentPosition position, ref ComponentCollisionLine coll)
        {
            if (CameraCollision(ref entity, ref position, ref coll))
                collisionManager.CollisionBetweenCamera(ref entity, COLLISIONTYPE.LINE_LINE);
        }

        private bool CameraCollision(ref Entity entity, ref ComponentPosition position, ref ComponentCollisionLine coll)
        {
            Vector2 wallSize;
            bool collisionX = false;
            bool collisionZ = false;

            // if horizontal wall
            if (coll.Direction == ComponentCollisionLine.DIRECTION.HORIZONTAL)
            {
                wallSize = new Vector2(coll.Length * 2, -0.5f);
                Vector2 wallStartingPosition = new Vector2(position.Position.X - coll.Length, position.Position.Z);

                collisionX = camera.cameraPosition.X >= wallStartingPosition.X && wallStartingPosition.X + wallSize.X >= camera.cameraPosition.X;

                if (position.Position.Z < 0)
                {
                    collisionZ = camera.cameraPosition.Z <= wallStartingPosition.Y && wallStartingPosition.Y + wallSize.Y <= camera.cameraPosition.Z;
                }
                else
                {
                    collisionZ = camera.cameraPosition.Z >= wallStartingPosition.Y && wallStartingPosition.Y - wallSize.Y >= camera.cameraPosition.Z;
                }
            }
            else
            {
                wallSize = new Vector2(-0.5f, coll.Length * 2);
                Vector2 wallStartingPosition = new Vector2(position.Position.X, position.Position.Z - coll.Length);

                collisionZ = camera.cameraPosition.Z >= wallStartingPosition.Y && wallStartingPosition.Y + wallSize.Y >= camera.cameraPosition.Z;

                if (position.Position.Z < 0)
                {
                    collisionX = camera.cameraPosition.X <= wallStartingPosition.X - wallSize.X && wallStartingPosition.X + wallSize.X <= camera.cameraPosition.X;
                }
                else
                {
                    collisionX = camera.cameraPosition.X >= wallStartingPosition.X && wallStartingPosition.X - wallSize.X >= camera.cameraPosition.X;
                }
            }
            return collisionX && collisionZ;
        }
    }
}
