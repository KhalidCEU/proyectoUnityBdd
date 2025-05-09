using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con UI
using UnityEngine.SceneManagement;

public class ManagerCanvas : MonoBehaviour
{

    public void LoadEmployees()
    {
        SceneManager.LoadScene("Employees");
    }
    public void LoadOrders()
    {
        SceneManager.LoadScene("Orders");
    }
    public void LoadProducts()
    {
        SceneManager.LoadScene("Products");
    }
    public void LoadCustomers()
    {
        SceneManager.LoadScene("Customers");
    }
    public void LoadStores()
    {
        SceneManager.LoadScene("Stores");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
