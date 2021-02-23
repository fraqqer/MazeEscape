using Ajax.Components;
using OpenTK;

namespace Ajax.Components
{
    public class ComponentVelocity : IComponent
    {
        Vector3 velocity;
        float deltaTime;

        public ComponentVelocity(float x, float y, float z, float dt)
        {
            velocity = new Vector3(x, y, z);
            this.deltaTime = dt;
        }

        public ComponentVelocity(Vector3 pos)
        {
            velocity = pos;
        }

        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public float DeltaTime
        {
            get { return this.deltaTime; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_VELOCITY; }
        }

        public void Close()
        {

        }
    }
}
