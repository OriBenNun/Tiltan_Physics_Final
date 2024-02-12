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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (_isOnGround)
        {
            return;
        }
        
        _force += Vector3.down * (mass * gravity);
        
        transform.Translate(Time.fixedDeltaTime * _force);
    }
    
    private void Jump()
    {
        if (_isOnGround)
        {
            _isOnGround = false;
        }
        
        Debug.Log($"Jumped! Force: {_force}");
        _force = Vector3.up * (mass * jumpForce);
        transform.Translate(_force);
    }
    
    private void OnCollisionStay(Collision other)
    {
        if (transform.position.y < 0f)
        {
            _isOnGround = true;
            var position = transform.position;
            position.y = 0.5f;
            transform.position = position;
        }
    }
}
