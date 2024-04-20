using Game.Enemy;
using TMPro;
using UnityEngine;

namespace Game
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text shipsSankText;
        
        private static int _currentScore;
        private void Awake()
        {
            _currentScore = 0;
            EnemyShip.OnEnemyShipSink += HandleOnEnemyShipSink;
        }

        private void Start()
        {
            UpdateText();
        }

        public static int GetCurrentScore() => _currentScore;

        private void OnDestroy()
        {
            EnemyShip.OnEnemyShipSink -= HandleOnEnemyShipSink;
        }
        private void UpdateText()
        {
            shipsSankText.text = $"Ships Sank: {_currentScore:N0}";
        }

        private void HandleOnEnemyShipSink()
        {
            _currentScore++;
            UpdateText();
        }
    }
}