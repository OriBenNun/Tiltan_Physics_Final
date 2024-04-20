using System;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyShip : MonoBehaviour
    {
        [SerializeField] private EnemyShipColliderManager colliderManager;

        public static event Action OnEnemyShipSink;
        public static event Action OnEnemyShipReachedCastle;

        private float _moveSpeed;

        private void Awake()
        {
            colliderManager.OnTookDamage += HandleOnTookDamage;
            colliderManager.OnCollidedWithCastle += HandleOnCollidedWithCastle;
        }

        private void Update()
        {
            MoveTowardsCastle();
        }
        
        private void OnDestroy()
        {
            colliderManager.OnTookDamage -= HandleOnTookDamage;
            colliderManager.OnCollidedWithCastle -= HandleOnCollidedWithCastle;
        }

        public void Init(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
        }

        private void MoveTowardsCastle()
        {
            transform.position += Vector3.back * (Time.deltaTime * _moveSpeed);
        }

        private void HandleOnTookDamage()
        {
            SinkShip();
        }

        private void SinkShip()
        {
            // TODO add VFX and sink animation
            
            OnEnemyShipSink?.Invoke();
            gameObject.SetActive(false);
        }
        
        private void HandleOnCollidedWithCastle()
        {
            OnEnemyShipReachedCastle?.Invoke();
        }
    }
}
