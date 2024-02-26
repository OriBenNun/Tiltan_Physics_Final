using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float mass = 10;
    [SerializeField] private float jumpForce = 5;
    
    private Vector3 _force;
    private bool _isOnGround;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ShootUp();
            ShootForward();
        }
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        
        transform.Translate(Time.fixedDeltaTime * _force);
    }

    private void ApplyGravity()
    {
        _force += Vector3.down * (mass * gravity);
    }

    private void ShootUp()
    {
        _force += Vector3.up * (mass * jumpForce);
    }
    
    private void ShootForward()
    {
        _force += Vector3.forward * (mass * jumpForce);
    }
}
