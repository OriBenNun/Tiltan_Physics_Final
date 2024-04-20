using Game.Enemy;
using TMPro;
using UnityEngine;

namespace Game
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text shipsSankText;
        
        private int _currentScore;
        private void Awake()
        {
            EnemyShip.OnEnemyShipSink += HandleOnEnemyShipSink;
        }

        private void Start()
        {
            UpdateText();
        }


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