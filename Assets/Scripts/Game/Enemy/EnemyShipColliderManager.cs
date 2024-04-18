using System;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyShipColliderManager : MonoBehaviour
    {
        public event Action OnTookDamage;
        public void TakeDamage()
        {
            OnTookDamage?.Invoke();
        }
    }
}