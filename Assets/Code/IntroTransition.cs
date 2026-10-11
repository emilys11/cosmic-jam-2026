using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroTransition : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void GoToGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
