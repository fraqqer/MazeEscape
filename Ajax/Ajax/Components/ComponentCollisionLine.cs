using Ajax.Components;
using OpenTK;

namespace Ajax.Components
{
    public class ComponentCollisionLine : IComponent
    {
        public enum DIRECTION
        {
            HORIZONTAL,
            VERTICAL
        }

        float length;
        DIRECTION direction;

        public ComponentCollisionLine(DIRECTION pDirection, float pLength)
        {
            length = pLength;
            direction = pDirection;
        }

        public float Length
        {
            get { return length; }
            set { length = value; }
        }

        public DIRECTION Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_LINE; }
        }

        public void Close()
        {

        }
    }
}
