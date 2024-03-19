using System;
using PhysicsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cannon
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] private float rotationSpeed = 1;
        [SerializeField] private RigidBody rigidBody;
        [SerializeField] private Transform cannonVisualTransform;
        [SerializeField] private float forwardDirectionCorrection;
        [SerializeField] private float maxRotationAngle = 45;
        [SerializeField] private float minRotationAngle = 15;
        [SerializeField] private float startingRotationAngle = 30;
        [SerializeField] private float maxHorizontalPosition = 10;
        [SerializeField] private float minHorizontalPosition = -10;
        [SerializeField] private float startingHorizontalPosition;

        public event Action<float> OnShootPressed;
        public event Action OnProjectileResetPressed;

        private GameControls _controls;

        private Vector3 _eulerRotation;

        private RotationDirection _currRotationDirection = RotationDirection.None;
        private MovementDirection _currMovementDirection = MovementDirection.None;
        
        private enum RotationDirection
        {
            Up,
            Down,
            None
        }
        
        private enum MovementDirection
        {
            Right,
            Left,
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
            _controls.Player.HorizontalMove.canceled += HandleHorizontalMoveCanceled;
            
            _controls.Player.VerticalRotation.performed += HandleVerticalRotationInput;
            _controls.Player.VerticalRotation.canceled += HandleVerticalRotationCanceled;

            _controls.Player.Shoot.performed += HandleShootInput;

            _controls.Debug.Reset.performed += HandleDebugResetInput;
        }

        private void Start()
        {
            UpdateRotation(startingRotationAngle);
            UpdatePosition(startingHorizontalPosition);
        }

        private void Update()
        {
            if (_currRotationDirection != RotationDirection.None)
            {
                RotateCannon(_currRotationDirection, rotationSpeed * Time.deltaTime);
            }
            
            if (_currMovementDirection != MovementDirection.None)
            {
                MoveCannon(_currMovementDirection, moveSpeed * Time.deltaTime);
            }
        }

        private void OnDestroy()
        {
            _controls.Player.HorizontalMove.performed -= HandleHorizontalMoveInput;
            _controls.Player.HorizontalMove.canceled -= HandleHorizontalMoveCanceled;
            
            _controls.Player.VerticalRotation.canceled -= HandleVerticalRotationCanceled;
            _controls.Player.VerticalRotation.performed -= HandleVerticalRotationInput;
            
            _controls.Player.Shoot.performed -= HandleShootInput;

            _controls.Debug.Reset.performed -= HandleDebugResetInput;
            
            _controls.Dispose();
        }

        public Vector3 GetCannonForwardDirection()
        {
            var forward = cannonVisualTransform.forward;
            return new Vector3(
                forward.x,
                forward.y + forwardDirectionCorrection,
                forward.z
                ).normalized;
        }

        private void MoveCannon(MovementDirection moveDirection, float speed)
        {
            var position = transform.position.x;
            switch (moveDirection)
            {
                case MovementDirection.Right:
                    position = Mathf.Min(maxHorizontalPosition, position + speed);
                    break;
                case MovementDirection.Left:
                    position = Mathf.Max(minHorizontalPosition, position - speed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null);
            }

            UpdatePosition(position);
        }

        private void UpdatePosition(float position)
        {
            var myTrans = transform;
            var pos = myTrans.position;
            
            pos = new Vector3(position, pos.y, pos.z);
            
            myTrans.position = pos;
        }

        private void RotateCannon(RotationDirection rotationDirection, float speed)
        {
            var rotation = _eulerRotation.x;
            switch (rotationDirection)
            {
                // It seems like the sides are flipped here, it's because rotating the cannon piece in the positive direction
                // make it "look downwards", and the other way around.
                case RotationDirection.Up:
                    rotation = Mathf.Max(minRotationAngle, rotation - speed);
                    break;
                case RotationDirection.Down:
                    rotation = Mathf.Min(maxRotationAngle, rotation + speed);
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
            if (obj.ReadValue<float>() < 0)
            {
                _currMovementDirection = MovementDirection.Left;
                return;
            }

            _currMovementDirection = MovementDirection.Right;
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
        private void HandleVerticalRotationCanceled(InputAction.CallbackContext obj)
        {
            _currRotationDirection = RotationDirection.None;
        }
        
        private void HandleHorizontalMoveCanceled(InputAction.CallbackContext obj)
        {
            _currMovementDirection = MovementDirection.None;
        }
        
        private void HandleShootInput(InputAction.CallbackContext obj)
        {
            // TODO change to spring charge
            OnShootPressed?.Invoke(300);
        }
        
        private void HandleDebugResetInput(InputAction.CallbackContext obj)
        {
            OnProjectileResetPressed?.Invoke();
        }
    }
}