using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    private bool gameIsPaused = false;
    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !gameIsPaused)
        {
            Pause();
            gameIsPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.P) && gameIsPaused)
        {
            Resume();
            gameIsPaused = false;
        }
    }
}
