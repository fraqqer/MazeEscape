using Ajax.Components;
using Ajax.OBJLoader;
using Ajax.Managers;

namespace Ajax.Components
{
    public class ComponentGeometry : IComponent
    {
        Geometry geometry;

        public ComponentGeometry(string geometryName)
        {
            this.geometry = ResourceManager.LoadGeometry(geometryName);
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_GEOMETRY; }
        }

        public Geometry Geometry()
        {
            return geometry;
        }

        public void Close()
        {

        }
    }
}
