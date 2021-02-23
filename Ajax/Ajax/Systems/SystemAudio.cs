using System.Collections.Generic;
using OpenTK.Audio.OpenAL;
using Ajax.Components;
using Ajax.Objects;
using Ajax.Systems;

namespace Ajax.Systems
{
    public class SystemAudio : ISystem
    { 
        // add velocity
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY | ComponentTypes.COMPONENT_ARTIFICIAL_INTELLIGENCE | ComponentTypes.COMPONENT_AUDIO);

        public string Name
        {
            get { return "SystemAudio"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent audioComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                ComponentAudio audio = (ComponentAudio)audioComponent;

                IComponent positionComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                ComponentPosition position = (ComponentPosition)positionComponent;

                IComponent velocityComponent = components.Find(delegate (IComponent component)
                {
                    return (component.ComponentType == ComponentTypes.COMPONENT_VELOCITY || component.ComponentType == ComponentTypes.COMPONENT_ARTIFICIAL_INTELLIGENCE);
                });

                Audio(ref audio, ref position, ref velocityComponent);
            }
        }

        public void Audio(ref ComponentAudio audio, ref ComponentPosition position, ref IComponent velocity)
        {
            if (velocity.ComponentType == ComponentTypes.COMPONENT_VELOCITY)
                audio.Velocity = ((ComponentVelocity)velocity).Velocity;
            else
                audio.Velocity = ((ComponentArtificialIntelligence)velocity).Velocity;

            audio.Position = position.Position;

            AL.Source(audio.Source, ALSource3f.Position, audio.Position.X, audio.Position.Y, audio.Position.Z);
            AL.Source(audio.Source, ALSource3f.Velocity, audio.Velocity.X, audio.Velocity.Y, audio.Velocity.Z);
            AL.DopplerFactor(17f);
        }
    }
}
