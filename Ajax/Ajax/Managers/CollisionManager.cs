using System.Collections.Generic;
using Ajax.Objects;

namespace Ajax.Managers
{
    public enum COLLISIONTYPE
    {
        SPHERE_SPHERE,
        LINE_LINE
    }

    public struct Collision
    {
        public Entity entity;
        public COLLISIONTYPE collisionType;
    }

    public abstract class CollisionManager
    {
        protected List<Collision> collisionManifold = new List<Collision>();
        public CollisionManager() { }

        public void ClearManifold() { collisionManifold.Clear(); }

        public void CollisionBetweenCamera(ref Entity entity, COLLISIONTYPE collisionType)
        {
            foreach (Collision coll in collisionManifold)
            {
                if (coll.entity == entity) return;
            }
            Collision collision;
            collision.entity = entity;
            collision.collisionType = collisionType;
            collisionManifold.Add(collision);
        }
        public abstract void ProcessCollisions();
    }
}