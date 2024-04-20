using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private int gameSceneIndex = 1;

        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void HandleStartGameButtonPressed()
        {
            SceneManager.LoadSceneAsync(gameSceneIndex);
        }
        
        public void HandleExitButtonPressed()
        {
            Application.Quit();
        }
    }
}
