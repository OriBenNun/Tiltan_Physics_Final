using PhysicsSystem;
using UnityEngine;

namespace Game
{
    public class Projectile : RigidBody
    {
        // [SerializeField] private RigidBody rb;
        
        public void Shoot(Vector3 force)
        {
            UseGravity();
            AddForce(force);
        }

        // TODO temp for debug
        public void ResetProjectile(Vector3 position)
        {
            SetPosition(position);
            UseGravity(false);
            ResetVelocity();
        }
    }
}
