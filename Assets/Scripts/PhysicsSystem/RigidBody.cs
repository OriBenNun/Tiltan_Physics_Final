using UnityEngine;

namespace PhysicsSystem
{
    public class RigidBody : MonoBehaviour
    {
        [SerializeField] private PhysicsManager physicsManager;
        [SerializeField] private bool useGravity;
        [SerializeField] private float mass = 1;

        private const float Gravity = 9.81f;
        
        // private Vector3 _force;
        private Vector3 _velocity;

        private void OnValidate()
        {
            physicsManager ??= FindObjectOfType<PhysicsManager>();
        }

        private void FixedUpdate()
        {
            if (useGravity)
            {
                ApplyGravity();
            }
            
            transform.Translate(Time.fixedDeltaTime * _velocity);
        }
        
        private void ApplyGravity()
        {
            AddForce(Vector3.down * (Gravity * mass));
        }

        public void UseGravity(bool b = true) => useGravity = b;
        
        public void AddForce(Vector3 force)
        {
            var acceleration = physicsManager.GetTimeScale() * (force / mass);
            _velocity += acceleration;
        }
        
        public void StopAllForces() 
        {
            _velocity = Vector3.zero;
        }
    }
}
