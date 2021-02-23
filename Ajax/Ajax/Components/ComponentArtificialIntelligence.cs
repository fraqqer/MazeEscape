using Ajax.Components;
using OpenTK;

namespace Ajax.Components
{
    public class ComponentArtificialIntelligence : IComponent
    {
        private Vector3 velocity;
        private float dt;
        private bool droneEnabled;

        public ComponentArtificialIntelligence(Vector3 velocity, float dt, bool droneEnabled)
        {
            this.Velocity = velocity;
            this.dt = dt;
            this.droneEnabled = droneEnabled;
        }

        public ComponentArtificialIntelligence(float x, float y, float z, float dt, bool droneEnabled) : this(new Vector3(x, y, z), dt, droneEnabled) { }

        public Vector3 Velocity
        {
            get { return this.velocity; }
            set { velocity = value; }
        }

        public float DeltaTime
        {
            get { return this.dt; }
        }

        public bool DroneEnabled
        {
            get { return this.droneEnabled; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_ARTIFICIAL_INTELLIGENCE; }
        }
        public void Close()
        {

        }
    }
}
