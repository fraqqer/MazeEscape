using Ajax.Components;
using OpenTK;

namespace Ajax.Components
{
    public class ComponentScale : IComponent
    {
        Vector3 scale;

        public ComponentScale(float x, float y, float z)
        {
            scale = new Vector3(x, y, z);
        }

        public ComponentScale(Vector3 sca)
        {
            scale = sca;
        }

        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SCALE; }
        }

        public void Close()
        {

        }
    }
}
