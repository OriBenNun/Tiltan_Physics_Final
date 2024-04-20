using System;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyShipColliderManager : MonoBehaviour
    {
        private const string CastleTagName = "Castle";
        public event Action OnTookDamage;
        public event Action OnCollidedWithCastle;
        public void TakeDamage()
        {
            OnTookDamage?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(CastleTagName))
            {
                OnCollidedWithCastle?.Invoke();
            }
        }
    }
}