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

            transform.Translate(_velocity);
        }
        
        public void AddForce(Vector3 forceToAdd)
        {
            var acceleration = forceToAdd / mass * physicsManager.GetFixedDeltaTimeScale();
            _velocity += acceleration;
        }

        public void UseGravity(bool b = true) => useGravity = b;
        
        public void ResetVelocity() => _velocity = Vector3.zero;

        public void SetMass(float newMass) => mass = newMass;

        private void ApplyGravity() => AddForce(Vector3.down * (Gravity * mass));
    }
}
