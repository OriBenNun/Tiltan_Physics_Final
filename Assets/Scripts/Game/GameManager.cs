using System;
using Game.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private RectTransform loseScreenUi;
        [SerializeField] private RectTransform gameplayScoreUi;
        [SerializeField] private RectTransform gameplaySpringUi;
        [SerializeField] private TMP_Text loseScoreText;
        [SerializeField] private TMP_Text losePersonalBestText;
        [SerializeField] private int gameSceneIndex = 1;
        [SerializeField] private int mainMenuSceneIndex = 0;

        private const string PersonalBestPlayerPrefsKey = "PersonalBestScore";

        public static event Action OnGameOver;
        private void Awake()
        {
            EnemyShip.OnEnemyShipReachedCastle += HandleOnEnemyShipReachedCastle;
        }

        private void Start()
        {
            StartGame();
        }

        private void OnDestroy()
        {
            EnemyShip.OnEnemyShipReachedCastle -= HandleOnEnemyShipReachedCastle;
        }

        public void HandleRetryButtonPressed()
        {
            SceneManager.LoadSceneAsync(gameSceneIndex);
        }
        
        public void HandleMainMenuButtonPressed()
        {
            SceneManager.LoadSceneAsync(mainMenuSceneIndex);
        }
        
        private void StartGame()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            gameplayScoreUi.gameObject.SetActive(true);
            gameplaySpringUi.gameObject.SetActive(true);
            loseScreenUi.gameObject.SetActive(false);
        }
        
        private void HandleOnEnemyShipReachedCastle()
        {
            StopGame();
            OpenLoseScreen();
        }

        private void OpenLoseScreen()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            var score = ScoreManager.GetCurrentScore();
            loseScoreText.text = $"Enemy Ships Sank: {score:N0}";
            
            var best = UpdatePersonalBest(score);
            losePersonalBestText.text = $"Personal Best: {best:N0}";
            
            loseScreenUi.gameObject.SetActive(true);
        }

        private static int UpdatePersonalBest(int score)
        {
            if (PlayerPrefs.HasKey(PersonalBestPlayerPrefsKey))
            {
                var currentBest = PlayerPrefs.GetInt(PersonalBestPlayerPrefsKey);
                currentBest = currentBest > score ? currentBest : score;
                PlayerPrefs.SetInt(PersonalBestPlayerPrefsKey, currentBest);
                return currentBest;
            }
            
            PlayerPrefs.SetInt(PersonalBestPlayerPrefsKey, score);
            return score;
        }

        private void StopGame()
        {
            gameplayScoreUi.gameObject.SetActive(false);
            gameplaySpringUi.gameObject.SetActive(false);
            Time.timeScale = 0;
            OnGameOver?.Invoke();
        }
    }
}
