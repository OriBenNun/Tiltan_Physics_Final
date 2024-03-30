using System;
using PhysicsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cannon
{
    public class CannonController : MonoBehaviour
    {
        [Header("Core Configuration")]
        [SerializeField] private Transform cannonVisualTransform;
        [SerializeField] private RigidBody rigidBody;
        
        [Header("Cannon Locomotion")]
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] private float rotationSpeed = 1;
        [SerializeField] private float forwardDirectionCorrection;
        [SerializeField] private float startingRotationAngle = 30;
        [SerializeField] private float startingHorizontalPosition;
        
        [Header("Constraints")]
        [SerializeField] private float maxRotationAngle = 45;
        [SerializeField] private float minRotationAngle = 15;
        [SerializeField] private float maxHorizontalPosition = 10;
        [SerializeField] private float minHorizontalPosition = -10;

        [Header("Spring")]
        [SerializeField] private float springAddTensionSpeed = 1;
        [SerializeField] private SpringConfigSo springConfigSo;
        [SerializeField] private Spring spring;

        public event Action<float> OnShootPressed;
        public event Action OnProjectileResetPressed;

        private GameControls _controls;

        private Vector3 _eulerRotation;

        private RotationDirection _currRotationDirection = RotationDirection.None;
        private MovementDirection _currMovementDirection = MovementDirection.None;
        private SpringAction _currSpringAction = SpringAction.None;

        private void OnValidate()
        {
            rigidBody ??= GetComponent<RigidBody>();
            spring ??= GetComponent<Spring>();
        }

        private void Awake()
        {
            _controls = new GameControls();
            
            _controls.Enable();

            _controls.Player.HorizontalMove.performed += HandleHorizontalMoveInput;
            _controls.Player.HorizontalMove.canceled += HandleHorizontalMoveCanceled;
            
            _controls.Player.VerticalRotation.performed += HandleVerticalRotationInput;
            _controls.Player.VerticalRotation.canceled += HandleVerticalRotationCanceled;

            _controls.Player.AddTension.performed += HandleAddTensionInput;
            _controls.Player.AddTension.canceled += HandleAddTensionCanceled;

            _controls.Player.Shoot.performed += HandleShootInput;

            _controls.Debug.Reset.performed += HandleDebugResetInput;
        }

        private void Start()
        {
            UpdateRotation(startingRotationAngle);
            UpdatePosition(startingHorizontalPosition);
            
            spring.InitSpring(springConfigSo);
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
            
            if (_currSpringAction != SpringAction.None)
            {
                AddTensionToSpring(_currSpringAction, springAddTensionSpeed * Time.deltaTime);
            }
        }

        private void OnDestroy()
        {
            _controls.Player.HorizontalMove.performed -= HandleHorizontalMoveInput;
            _controls.Player.HorizontalMove.canceled -= HandleHorizontalMoveCanceled;
            
            _controls.Player.VerticalRotation.canceled -= HandleVerticalRotationCanceled;
            _controls.Player.VerticalRotation.performed -= HandleVerticalRotationInput;
            
            _controls.Player.AddTension.performed -= HandleAddTensionInput;
            _controls.Player.AddTension.canceled -= HandleAddTensionCanceled;
            
            _controls.Player.Shoot.performed -= HandleShootInput;

            _controls.Debug.Reset.performed -= HandleDebugResetInput;
            
            _controls.Dispose();
        }
        
        private void AddTensionToSpring(SpringAction currSpringAction, float speed)
        {
            var displacementToAdd = 0f;
            switch (currSpringAction)
            {
                case SpringAction.AddTension:
                    displacementToAdd += speed;
                    break;
                case SpringAction.SubtractTension:
                    displacementToAdd -= speed;
                    break;
                case SpringAction.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            spring.AddDisplacement(displacementToAdd);
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
        private void HandleAddTensionInput(InputAction.CallbackContext obj)
        {
            if (obj.ReadValue<float>() < 0)
            {
                _currSpringAction = SpringAction.SubtractTension;
                return;
            }

            _currSpringAction = SpringAction.AddTension;
        }
        
        private void HandleVerticalRotationCanceled(InputAction.CallbackContext obj)
        {
            _currRotationDirection = RotationDirection.None;
        }
        
        private void HandleHorizontalMoveCanceled(InputAction.CallbackContext obj)
        {
            _currMovementDirection = MovementDirection.None;
        }
        private void HandleAddTensionCanceled(InputAction.CallbackContext obj)
        {
            _currSpringAction = SpringAction.None;
        }
        
        private void HandleShootInput(InputAction.CallbackContext obj)
        {
            OnShootPressed?.Invoke(spring.GetForceAndReleaseTension());
        }
        
        private void HandleDebugResetInput(InputAction.CallbackContext obj)
        {
            OnProjectileResetPressed?.Invoke();
        }
        
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

        private enum SpringAction
        {
            AddTension,
            SubtractTension,
            None
        }
    }
}