using PhysicsSystem;
using UnityEngine;

namespace Game
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private RigidBody rb;
        [SerializeField] private float mass = 10f;
        
        private void OnValidate()
        {
            rb ??= GetComponent<RigidBody>();
        }

        private void Awake()
        {
            rb.SetMass(mass);
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
            rb.ResetVelocity();
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
