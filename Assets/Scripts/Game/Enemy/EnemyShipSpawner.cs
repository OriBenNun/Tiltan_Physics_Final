using System.Collections;
using Game.Cannon;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemy
{
    public class EnemyShipSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyShip shipPrefab;
        [SerializeField] private float initialSpawnCooldown = 4.5f;
        [SerializeField] private float difficultySpawnCooldownMultiplierPerMinute = 0.7f;
        [SerializeField] private float yToSpawn;
        [SerializeField] private float zToSpawn;

        private int _currentDifficulty = 1;
        private float _currentGameTimer;
        private float _currentSpawnCooldown;

        private Coroutine _spawnRoutine;

        private void Awake()
        {
            _currentGameTimer = 0;
        }

        private void Start()
        {
            UpdateCooldown(initialSpawnCooldown);
        }

        private void FixedUpdate()
        {
            _currentGameTimer += Time.fixedDeltaTime;
            
            if (!(_currentGameTimer >= _currentDifficulty * 60)) return;
            
            _currentDifficulty++;
            UpdateCooldown(_currentSpawnCooldown * difficultySpawnCooldownMultiplierPerMinute);
        }

        private void UpdateCooldown(float newCooldown)
        {
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
            }
            
            _currentSpawnCooldown = newCooldown;

            _spawnRoutine = StartCoroutine(SpawnShipByCooldownRoutine());
        }

        private IEnumerator SpawnShipByCooldownRoutine()
        {
            while (true)
            {
                var randomPosition = GetRandomPositionWithinMap();
                SpawnShip(randomPosition);

                yield return new WaitForSeconds(_currentSpawnCooldown);
            }
        }
        
        private void SpawnShip(Vector3 randomPosition)
        {
            var ship = Instantiate(shipPrefab, randomPosition, Quaternion.identity, transform);
            ship.transform.Rotate(Vector3.up, 270);
        }

        private Vector3 GetRandomPositionWithinMap()
        {
            var maxX = CannonController.GetMaxHorizontalPosition();
            var minX = CannonController.GetMinHorizontalPosition();
            var x = Random.Range(minX, maxX);
            return new Vector3(x, yToSpawn, zToSpawn);
        }
    }
}
