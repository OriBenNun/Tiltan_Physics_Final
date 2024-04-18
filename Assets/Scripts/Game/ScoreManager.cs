using Game.Enemy;
using UnityEngine;

namespace Game
{
    public class ScoreManager : MonoBehaviour
    {
        private int _currentScore;
        private void Awake()
        {
            EnemyShip.OnEnemyShipSink += HandleOnEnemyShipSink;
        }

        private void OnDestroy()
        {
            EnemyShip.OnEnemyShipSink -= HandleOnEnemyShipSink;
        }

        private void HandleOnEnemyShipSink()
        {
            _currentScore++;
            Debug.Log($"Your current score is: {_currentScore}");
        }
    }
}