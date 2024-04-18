using Game.Cannon;
using UnityEngine;

namespace PhysicsSystem
{
    public class PhysicsManager : MonoBehaviour
    {
        [SerializeField] private float initialTimeScale = 1f;
        [SerializeField] private float afterShotTimeScale = 0.4f;
        
        private float _currentTimeScale;

        private void Awake()
        {
            CannonController.OnShootPressed += HandleOnProjectileShot;
            ProjectileManager.OnProjectileReset += HandleOnProjectileReset;
            
            _currentTimeScale = initialTimeScale;
        }

        private void Start()
        {
            UpdateTimeScale();
        }

        private void OnDestroy()
        {
            CannonController.OnShootPressed -= HandleOnProjectileShot;
            ProjectileManager.OnProjectileReset -= HandleOnProjectileReset;
        }

        private void HandleOnProjectileReset()
        {
            ChangeTimeScale(initialTimeScale);
        }

        private void HandleOnProjectileShot(float _)
        {
            ChangeTimeScale(afterShotTimeScale);
        }

        private void ChangeTimeScale(float timeScale)
        {
            _currentTimeScale = timeScale;
            UpdateTimeScale();
        }

        private void UpdateTimeScale()
        {
            Time.timeScale = _currentTimeScale;
        }
    }
}