using System.Collections;
using Game.Cannon;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemy
{
    public class EnemyShipSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyShip shipPrefab;
        [SerializeField] private float secondsPerDifficultyLevel = 10f;
        [SerializeField] private float initialSpawnCooldown = 4.5f;
        [SerializeField] private float difficultySpawnCooldownMultiplierPerLevel = 0.7f;
        [SerializeField] private float yToSpawn;
        [SerializeField] private float minZToSpawn = 100;
        [SerializeField] private float maxZToSpawn = 200;
        [SerializeField] private float initialMoveSpeed = 2f;
        [SerializeField] private float difficultyMoveSpeedMultiplierPerLevel = 1.5f;


        private int _currentDifficultyLevel = 1;
        private float _currentMoveSpeed;
        private float _currentGameTimer;
        private float _currentSpawnCooldown;

        private Coroutine _spawnRoutine;

        private void Awake()
        {
            _currentMoveSpeed = initialMoveSpeed;
            _currentGameTimer = 0;
        }

        private void Start()
        {
            UpdateCooldown(initialSpawnCooldown);
        }

        private void FixedUpdate()
        {
            _currentGameTimer += Time.fixedDeltaTime;
            
            if (!(_currentGameTimer >= _currentDifficultyLevel * secondsPerDifficultyLevel)) return;
            
            _currentDifficultyLevel++;
            _currentMoveSpeed *= difficultyMoveSpeedMultiplierPerLevel;
            UpdateCooldown(_currentSpawnCooldown * difficultySpawnCooldownMultiplierPerLevel);
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
            ship.Init(_currentMoveSpeed);
        }

        private Vector3 GetRandomPositionWithinMap()
        {
            var maxX = CannonController.GetMaxHorizontalPosition();
            var minX = CannonController.GetMinHorizontalPosition();
            var x = Random.Range(minX, maxX);
            var z = Random.Range(minZToSpawn, maxZToSpawn);
            return new Vector3(x, yToSpawn, z);
        }
    }
}
