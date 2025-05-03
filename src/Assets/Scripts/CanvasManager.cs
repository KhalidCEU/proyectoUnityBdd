using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con UI
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    private static CanvasManager instance;

    void Start()
    {
        Debug.Log("Launching...");
    }

    void Update()
    {

    }

    public void LoadScene(string sceneName) {
        Debug.Log("Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
