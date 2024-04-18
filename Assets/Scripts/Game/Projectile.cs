using System;
using PhysicsSystem;
using UnityEngine;

namespace Game
{
    public class Projectile : RigidBody
    {
        public event Action<Collider> OnProjectileCollided;
        
        private void OnTriggerEnter(Collider other)
        {
            OnProjectileCollided?.Invoke(other);
        }

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
