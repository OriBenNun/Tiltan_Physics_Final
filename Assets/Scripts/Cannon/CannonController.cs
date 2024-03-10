using System;
using PhysicsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cannon
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] private float rotationSpeed = 1;
        [SerializeField] private RigidBody rigidBody;
        [SerializeField] private Transform cannonVisualTransform;
        [SerializeField] private float maxRotationAngle = 45;
        [SerializeField] private float minRotationAngle = 15;
        [SerializeField] private float startingRotationAngle = 30;

        private GameControls _controls;

        private Vector3 _eulerRotation;

        private RotationDirection _currRotationDirection = RotationDirection.None;
        
        private enum RotationDirection
        {
            Up,
            Down,
            None
        }

        private void OnValidate()
        {
            rigidBody ??= GetComponent<RigidBody>();
        }

        private void Awake()
        {
            _controls = new GameControls();
            
            _controls.Enable();

            _controls.Player.HorizontalMove.performed += HandleHorizontalMoveInput;
            _controls.Player.VerticalRotation.performed += HandleVerticalRotationInput;
            _controls.Player.VerticalRotation.canceled += HandleVerticalRotationCanceled;
        }

        private void Start()
        {
            // _currentRotation = cannonVisualTransform.rotation;
            UpdateRotation(startingRotationAngle);
        }

        private void Update()
        {
            if (_currRotationDirection == RotationDirection.None)
            {
                return;
            }
            
            RotateCannon(_currRotationDirection, rotationSpeed * Time.deltaTime);
        }

        private void OnDestroy()
        {
            _controls.Player.HorizontalMove.performed -= HandleHorizontalMoveInput;
            _controls.Player.VerticalRotation.performed -= HandleVerticalRotationInput;
            
            _controls.Dispose();
        }

        private void HandleVerticalRotationInput(InputAction.CallbackContext obj)
        {
            if (obj.ReadValue<float>() < 0)
            {
                _currRotationDirection = RotationDirection.Down;
                return;
            }

            _currRotationDirection = RotationDirection.Up;
        }

        private void RotateCannon(RotationDirection rotationDirection, float speed)
        {
            var rotation = _eulerRotation.x;
            switch (rotationDirection)
            {
                case RotationDirection.Up:
                    rotation = Mathf.Min(maxRotationAngle, rotation + speed);
                    break;
                case RotationDirection.Down:
                    rotation = Mathf.Max(minRotationAngle, rotation - speed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotationDirection), rotationDirection, null);
            }

            UpdateRotation(rotation);
        }

        private void UpdateRotation(float rotation)
        {
            _eulerRotation = new Vector3(rotation, _eulerRotation.y, _eulerRotation.z);
            cannonVisualTransform.eulerAngles = _eulerRotation;
        }

        private void HandleHorizontalMoveInput(InputAction.CallbackContext obj)
        {
            
        }
        
        private void HandleVerticalRotationCanceled(InputAction.CallbackContext obj)
        {
            _currRotationDirection = RotationDirection.None;
        }
    }
}