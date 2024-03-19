using UnityEngine;

namespace PhysicsSystem
{
    public class RigidBody : MonoBehaviour
    {
        [SerializeField] private bool useGravity;
        [SerializeField] private float mass = 1;
        [SerializeField] private float timeScale = 0.2f;

        private const float Gravity = 9.81f;

        private const float MaxForceMagnitude = 20f;
        
        private Vector3 _force;

        private void FixedUpdate()
        {
            if (useGravity)
            {
                ApplyGravity();
            }
        
            transform.Translate(Time.fixedDeltaTime * timeScale * _force);
        }
        
        private void ApplyGravity()
        {
            AddForce(Vector3.down * (mass * Gravity));
        }

        public void UseGravity(bool b = true) => useGravity = b;
        
        public void AddForce(Vector3 force, bool clamped = false)
        {
            if (clamped)
            {
                _force = Vector3.ClampMagnitude(_force + force, MaxForceMagnitude);
            }

            _force += force;
        }
        
        public void ChangeForce(Vector3 force, bool clamped = false) 
        {
            if (clamped)
            {
                _force = Vector3.ClampMagnitude(force, MaxForceMagnitude);
            }

            _force = force;
        }
    }
}
