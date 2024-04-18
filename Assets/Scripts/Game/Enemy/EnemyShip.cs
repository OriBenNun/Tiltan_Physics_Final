using System;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyShip : MonoBehaviour
    {
        [SerializeField] private EnemyShipColliderManager colliderManager;

        public static event Action OnEnemyShipSink;
        private void Awake()
        {
            colliderManager.OnTookDamage += HandleOnTookDamage;
        }

        private void OnDestroy()
        {
            colliderManager.OnTookDamage -= HandleOnTookDamage;
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
    }
}
