using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float mass = 10;
    [SerializeField] private float bounciness = 0.7f;
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

    // Bounciness formula Vf = - (E * Vi)
    // Where Vf is Final Velocity, E is bounciness factor [between 0 to 1] and Vi is Initial Velocity
    private void OnCollisionEnter(Collision other)
    {
        _force = -(bounciness * _force);
    }

    private void OnCollisionStay(Collision other)
    {
        Debug.Log($"HERE");
        if (transform.position.y < 0.4f)
        {
            _isOnGround = true;
            var position = transform.position;
            position.y = 0.5f;
            transform.position = position;
        }
    }
}
