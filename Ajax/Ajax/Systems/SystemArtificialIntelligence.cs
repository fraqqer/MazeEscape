using Ajax;
using Ajax.Components;
using Ajax.Objects;
using Ajax.Systems;
using OpenTK;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ajax.Systems
{
    public class SystemArtificialIntelligence : ISystem
    {
        private Camera camera;

        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_ARTIFICIAL_INTELLIGENCE);

        public SystemArtificialIntelligence(ref Camera camera)
        {
            this.camera = camera;
        }

        public string Name
        {
            get { return "SystemArtificialIntelligence"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentPosition)positionComponent).Position;

                IComponent aiComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_ARTIFICIAL_INTELLIGENCE;
                });
                ComponentArtificialIntelligence ai = ((ComponentArtificialIntelligence)aiComponent);

                Debugger.Break();

                if (ai.DroneEnabled && entity.Name == "Drone")
                    ((ComponentPosition)positionComponent).Position = DroneAI(ref entity, ref position, ref ai);

                if (entity.Name == "Room One Ball" || entity.Name == "Room Two Ball")
                    ((ComponentArtificialIntelligence)aiComponent).Velocity = BallAI(ref entity, ref position, ai.Velocity);
            }
        }

        private Vector3 DroneAI(ref Entity entity, ref Vector3 position, ref ComponentArtificialIntelligence ai)
        {
            if (camera.cameraPosition.X > position.X)
                position.X += ai.Velocity.X * ai.DeltaTime;
            else
                position.X -= ai.Velocity.X * ai.DeltaTime;

            if (camera.cameraPosition.Z > position.Z)
                position.Z += ai.Velocity.Z * ai.DeltaTime;
            else
                position.Z -= ai.Velocity.Z * ai.DeltaTime;

            return position;
        }

        private Vector3 BallAI(ref Entity entity, ref Vector3 position, Vector3 ballVelocity)
        {
            if (entity.Name == "Room One Ball")
            {
                if (position.Z > -5 || position.Z < -11) return ballVelocity = new Vector3(ballVelocity.X, ballVelocity.Y, -(ballVelocity.Z));
            }
            else
            {
                return ballVelocity;
            }

            return position;
        }
    }
}