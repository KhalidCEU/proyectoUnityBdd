using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerItemUI : MonoBehaviour
{
    private Customer customerData;
    private CustomerManager manager;

    public TMP_Text nameText;

    public void Setup(Customer customer, CustomerManager mgr)
    {
        // Debug.Log(" Setup() iniciado para: " + (customer != null ? customer.Name : "customer NULL"));

        customerData = customer;
        manager = mgr;

        try
        {
            nameText.text = $"{customer.Id}. {customer.Name}, {customer.Email}";
            // Debug.Log(" Texto asignado: " + nameText.text);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(" Error al asignar texto: " + ex.Message);
        }

        try
        {
            GetComponentInChildren<Button>().onClick.AddListener(() => {
                manager.OpenDetailPopup(customerData);
            });
            Debug.Log(" Botón conectado correctamente.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError(" Error asignando el botón: " + ex.Message);
        }
    }
}
