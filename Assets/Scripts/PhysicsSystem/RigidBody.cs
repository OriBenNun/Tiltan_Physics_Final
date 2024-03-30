using UnityEngine;

namespace PhysicsSystem
{
    public class RigidBody : MonoBehaviour
    {
        [SerializeField] private bool useGravity;
        [SerializeField] private float mass = 1;

        private const float Gravity = 9.81f;
        
        private Vector3 _force;

        private void FixedUpdate()
        {
            if (useGravity)
            {
                ApplyGravity();
            }
        
            transform.Translate(Time.fixedDeltaTime * PhysicsManager.TimeScale * _force);
        }
        
        private void ApplyGravity()
        {
            AddForce(Vector3.down * (mass * Gravity), true);
        }

        public void UseGravity(bool b = true) => useGravity = b;
        
        public void AddForce(Vector3 force, bool clamped = false)
        {
            if (clamped)
            {
                Debug.Log($"Clamped! {_force + force}");
                _force = Vector3.ClampMagnitude(_force + force, PhysicsManager.MaxForceMagnitude);
                return;
            }

            _force += force;
        }
        
        public void ChangeForce(Vector3 force, bool clamped = false) 
        {
            if (clamped)
            {
                _force = Vector3.ClampMagnitude(force, PhysicsManager.MaxForceMagnitude);
                return;
            }

            _force = force;
        }
    }
}
