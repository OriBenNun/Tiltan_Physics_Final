using UnityEngine;

namespace Game.Cannon
{
    public class ProjectileManager : MonoBehaviour
    {
        [SerializeField] private float shootForce = 0.1f;
        [SerializeField] private CannonController controller;
        [SerializeField] private Projectile loadedProjectile;
        [SerializeField] private Transform shootOrigin;

        private bool _isLoaded;

        private void OnValidate()
        {
            controller ??= GetComponent<CannonController>();
        }

        private void Start()
        {
            if (loadedProjectile == null)
            {
                Debug.Log($"{name} is missing a projectile to shoot!");
                return;
            }

            ResetAndLoadProjectile();

            controller.OnShootPressed += HandleOnShootPressed;
            controller.OnProjectileResetPressed += HandleOnResetPressed;
        }

        private void LateUpdate()
        {
            if (!_isLoaded) { return; }
            
            UpdateProjectileToFollowCannon();
        }

        private void UpdateProjectileToFollowCannon()
        {
            loadedProjectile.SetPosition(shootOrigin.position);
        }

        private void HandleOnShootPressed()
        {
            _isLoaded = false;
            
            var dir = controller.GetCannonForwardDirection();
            var finalForce = dir * shootForce;
            loadedProjectile.Shoot(finalForce);
        }
        
        private void HandleOnResetPressed()
        {
            ResetAndLoadProjectile();
        }
        
        private void ResetAndLoadProjectile()
        {
            loadedProjectile.ResetProjectile(shootOrigin.position);
            _isLoaded = true;
        }
    }
}
