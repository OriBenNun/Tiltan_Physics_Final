using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private const float Gravity = 9.81f;

    [SerializeField] private float mass = 10;
    [SerializeField] private float startingForce;

    private Vector3 _force;
    
    // Start is called before the first frame update
    void Start()
    {
        _force = Vector3.up * (mass * startingForce);
        transform.Translate(_force);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // _velocity -= Gravity * Time.fixedDeltaTime;
        _force += Vector3.down * (mass * Gravity);
        
        transform.Translate(_force * Time.fixedDeltaTime);
    }
}
