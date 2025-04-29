using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con UI
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    // Variables públicas para los botones
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadProduct1()
    {
        SceneManager.LoadScene("Product1");
    }
    public void LoadProduct2()
    {
        SceneManager.LoadScene("Product2");
    }
    public void LoadProduct3()
    {
        SceneManager.LoadScene("Product3");
    }
    public void LoadProduct4()
    {
        SceneManager.LoadScene("Product4");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
