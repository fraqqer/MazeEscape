using Ajax.Components;
using OpenTK;

namespace Ajax.Components
{
    public class ComponentRotation : IComponent
    {
        Vector3 rotation;

        /// <summary>
        /// Each value must be in radians! Use MathHelper.DegreesToRadians() to help convert.
        /// </summary>
        /// <param name="x">Rotation in terms of the x axis.</param>
        /// <param name="y">Rotation in terms of the y axis.</param>
        /// <param name="z">Rotation in terms of the z axis.</param>
        public ComponentRotation(float x, float y, float z)
        {
            rotation = new Vector3(x, y, z);
        }

        /// <summary>
        /// Each value must be in radians! Use MathHelper.DegreesToRadians() to help convert.
        /// </summary>
        /// <param name="x">Rotation in terms of the x axis.</param>
        /// <param name="y">Rotation in terms of the y axis.</param>
        /// <param name="z">Rotation in terms of the z axis.</param>
        public ComponentRotation(Vector3 rot)
        {
            rotation = rot;
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_ROTATION; }
        }

        public void Close()
        {

        }
    }
}
