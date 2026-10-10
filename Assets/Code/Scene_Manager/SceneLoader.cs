
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Reload the current scene.
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Load another scene by its name.
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    
    public void QuitGame()
    {
        Application.Quit();
    }
}
