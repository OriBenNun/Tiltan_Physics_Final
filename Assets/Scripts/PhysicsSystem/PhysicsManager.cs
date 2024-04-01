using UnityEngine;
using UnityEngine.InputSystem;

namespace PhysicsSystem
{
    public class PhysicsManager : MonoBehaviour
    {
        [SerializeField] private float timeScale = 1f;

        private GameControls _controls;

        private void Awake()
        {
            _controls = new GameControls();
            _controls.Enable();

            _controls.Debug.UpdateTimeScale.performed += HandleOnUpdateTimeScale;
        }

        private void Start()
        {
            UpdateTimeScale();
        }

        private void HandleOnUpdateTimeScale(InputAction.CallbackContext _)
        {
            UpdateTimeScale();
        }

        private void UpdateTimeScale()
        {
            Time.timeScale = timeScale;
        }
    }
}