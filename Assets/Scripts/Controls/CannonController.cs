using PhysicsSystem;
using UnityEngine;

namespace Controls
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private float upForce = 10;
        [SerializeField] private float forwardForce = 10;
        [SerializeField] private RigidBody rigidBody;

        private void OnValidate()
        {
            rigidBody ??= GetComponent<RigidBody>();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.W)) return;

            ShootUp();
            ShootForward();
        }

        private void ShootUp()
        {
            rigidBody.AddForce(Vector3.up * upForce);
        }

        private void ShootForward()
        {
            rigidBody.AddForce(Vector3.forward * forwardForce);
        }
    }
}