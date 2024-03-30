using PhysicsSystem;
using UnityEngine;

namespace Game
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private RigidBody rb;
        
        private void OnValidate()
        {
            rb ??= GetComponent<RigidBody>();
        }

        public void Shoot(Vector3 force)
        {
            rb.UseGravity();
            rb.AddForce(force);
        }

        // TODO temp for debug
        public void ResetProjectile(Vector3 position)
        {
            transform.position = position;
            rb.UseGravity(false);
            rb.StopAllForces();
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
