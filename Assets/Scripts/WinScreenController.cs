using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenController : MonoBehaviour
{
    public GameObject winCanvas;

    void Awake()
    {
        winCanvas.SetActive(false);
    }

    public void ShowWinScreen()
    {
        winCanvas.SetActive(true);

        // Freeze the game
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
