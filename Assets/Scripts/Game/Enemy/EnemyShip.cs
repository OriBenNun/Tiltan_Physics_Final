using System;
using System.Collections;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyShip : MonoBehaviour
    {
        [SerializeField] private EnemyShipColliderManager colliderManager;
        [SerializeField] private ParticleSystem boomParticles;
        
        public static event Action OnEnemyShipSink;
        public static event Action OnEnemyShipReachedCastle;

        private float _moveSpeed;
        private static readonly int Die = Animator.StringToHash("Die");

        private bool _isDead;

        private void Awake()
        {
            colliderManager.OnTookDamage += HandleOnTookDamage;
            colliderManager.OnCollidedWithCastle += HandleOnCollidedWithCastle;
        }

        private void Update()
        {
            if (_isDead)
            {
                return;
            }
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
            if (_isDead)
            {
                return;
            }
            
            SinkShip();
        }

        private void SinkShip()
        {
            StartCoroutine(SinkAnimationRoutine());
            OnEnemyShipSink?.Invoke();
        }

        private IEnumerator SinkAnimationRoutine()
        {
            GetComponent<Animator>().SetTrigger(Die);
            boomParticles.Play();
            _isDead = true;
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }

        private void HandleOnCollidedWithCastle()
        {
            if (_isDead)
            {
                return;
            }
            OnEnemyShipReachedCastle?.Invoke();
        }
    }
}
