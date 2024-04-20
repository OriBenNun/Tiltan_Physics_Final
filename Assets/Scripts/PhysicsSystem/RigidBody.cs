using UnityEngine;

namespace PhysicsSystem
{
    public class RigidBody : MonoBehaviour
    {
        [SerializeField] private float mass = 1;
        
        private bool _useGravity;
        private const float Gravity = 9.81f;
        private Vector3 _velocity;

        private void Update()
        {
            transform.Translate(_velocity * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (_useGravity)
            {
                ApplyGravity();
            }
        }

        public void SetPosition(Vector3 position) => transform.position = position;

        private void ApplyGravity() => AddForce(Vector3.down * (Gravity * mass));

        protected void AddForce(Vector3 forceToAdd)
        {
            var acceleration = forceToAdd / mass;
            _velocity += acceleration;
        }

        protected void UseGravity(bool b = true) => _useGravity = b;

        protected void ResetVelocity() => _velocity = Vector3.zero;
    }
}
