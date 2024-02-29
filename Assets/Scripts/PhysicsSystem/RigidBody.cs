using UnityEngine;

namespace PhysicsSystem
{
    public class RigidBody : MonoBehaviour
    {
        [SerializeField] private bool isAffectedByGravity;
        [SerializeField] private float mass = 10;
    
        private const float _gravity = 9.81f;
        
        private Vector3 _force;
        private bool _isOnGround;

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.W))
        //     {
        //         ShootUp();
        //         ShootForward();
        //     }
        // }

        private void FixedUpdate()
        {
            if (isAffectedByGravity)
            {
                ApplyGravity();
            }
        
            transform.Translate(Time.fixedDeltaTime * _force);
        }

        private void ApplyGravity()
        {
            _force += Vector3.down * (mass * _gravity);
        }

        // private void ShootUp()
        // {
        //     _force += Vector3.up * (mass * jumpForce);
        // }
        //
        // private void ShootForward()
        // {
        //     _force += Vector3.forward * (mass * jumpForce);
        // }
    }
}
