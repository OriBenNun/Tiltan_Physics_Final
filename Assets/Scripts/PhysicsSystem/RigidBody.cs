using UnityEngine;

namespace PhysicsSystem
{
    public class RigidBody : MonoBehaviour
    {
        [SerializeField] private bool useGravity;
        [SerializeField] private float mass = 1;

        private const float Gravity = 9.81f;

        private const float MaxForceMagnitude = 20f;
        
        private Vector3 _force;

        private void FixedUpdate()
        {
            if (useGravity)
            {
                ApplyGravity();
            }
        
            transform.Translate(Time.fixedDeltaTime * _force);
        }
        
        private void ApplyGravity()
        {
            AddForce(Vector3.down * (mass * Gravity));
        }
        
        public void AddForce(Vector3 force)
        {
            _force = Vector3.ClampMagnitude(_force + force, MaxForceMagnitude);
        }
        
        public void ChangeForce(Vector3 force) 
        {
            _force = Vector3.ClampMagnitude(force, MaxForceMagnitude);
        }
    }
}
