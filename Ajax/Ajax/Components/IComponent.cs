using System;

namespace Ajax.Components
{
    [Flags]
    public enum ComponentTypes
    {
        COMPONENT_NONE = 0,
        COMPONENT_POSITION = 1 << 0,
        COMPONENT_GEOMETRY = 1 << 1,
        COMPONENT_TEXTURE = 1 << 2,
        COMPONENT_VELOCITY = 1 << 3,
        COMPONENT_AUDIO = 1 << 4,
        COMPONENT_COLLISION_SPHERE = 1 << 5,
        COMPONENT_COLLISION_LINE = 1 << 6,
        COMPONENT_ROTATION = 1 << 7,
        COMPONENT_SCALE = 1 << 8,
        COMPONENT_ARTIFICIAL_INTELLIGENCE = 1 << 9
    }

    public interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }

        void Close();
    }
}