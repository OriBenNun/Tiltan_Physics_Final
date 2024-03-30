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

            var dirToMove = _force.normalized;
            var amountToMove = Time.fixedDeltaTime * PhysicsManager.TimeScale * _force.magnitude;

            var finalMove = amountToMove * dirToMove;
            transform.Translate(finalMove);
        }
        
        private void ApplyGravity()
        {
            AddForce(Vector3.down * (mass * Gravity));
        }

        public void UseGravity(bool b = true) => useGravity = b;
        
        public void AddForce(Vector3 force, bool clamped = false)
        {
            _force += force;
        }
        
        public void ChangeForce(Vector3 force, bool clamped = false) 
        {
            _force = force;
        }
    }
}
