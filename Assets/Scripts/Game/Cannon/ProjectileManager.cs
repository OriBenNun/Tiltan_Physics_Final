using System;
using Cinemachine;
using Game.Enemy;
using UnityEngine;

namespace Game.Cannon
{
    public class ProjectileManager : MonoBehaviour
    {
        [SerializeField] private CannonController controller;
        [SerializeField] private Projectile loadedProjectile;
        [SerializeField] private Transform shootOrigin;
        [SerializeField] private CinemachineVirtualCamera ballFollowCamera;

        private const string ProjectileResetPlaneTagName = "ProjectileResetPlane";
        private const string EnemyShipTagName = "EnemyShip";

        public static event Action OnProjectileReset;
        
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
            loadedProjectile.OnProjectileCollided += HandleOnProjectileCollided;

            CannonController.OnShootPressed += HandleOnShootPressed;
            controller.OnProjectileResetPressed += HandleOnResetPressed;
        }

        private void HandleOnProjectileCollided(Collider other)
        {
            if (other.CompareTag(EnemyShipTagName) && other.TryGetComponent<EnemyShipColliderManager>(out var enemyShipColliderManager))
            {
                enemyShipColliderManager.TakeDamage();
                ResetAndLoadProjectile();
                return;
            }
            
            if (other.CompareTag(ProjectileResetPlaneTagName))
            {
                ResetAndLoadProjectile();
            }
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

        private void HandleOnShootPressed(float shootForce)
        {
            _isLoaded = false;
            
            var dir = controller.GetCannonForwardDirection();
            var finalForce = dir * shootForce;
            loadedProjectile.Shoot(finalForce);
            ballFollowCamera.gameObject.SetActive(true);
        }
        
        private void HandleOnResetPressed()
        {
            ResetAndLoadProjectile();
        }
        
        private void ResetAndLoadProjectile()
        {
            loadedProjectile.ResetProjectile(shootOrigin.position);
            _isLoaded = true;
            ballFollowCamera.gameObject.SetActive(false);
            OnProjectileReset?.Invoke();
        }
    }
}
