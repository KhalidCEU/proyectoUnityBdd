using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderItemUI : MonoBehaviour
{
    private Order orderData;
    private OrderManager manager;

    public TMP_Text nameText;

    public void Setup(Order order, OrderManager mgr)
    {
        // Debug.Log("Setup() iniciado para Order: " + (order != null ? order.Date : "orden NULL"));

        orderData = order;
        manager = mgr;

        if (nameText == null)
        {
            Debug.LogError("nameText está NULL en OrderItemUI");
        }
        else
        {
            Debug.Log("nameText no es null");
        }

        try
        {
            nameText.text = $"{order.Id}. Date: {order.Date} | Customer ID: {order.CustomerId}";
            // Debug.Log("Texto asignado: " + nameText.text);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error asignando texto: " + ex.Message);
        }

        try
        {
            GetComponentInChildren<Button>().onClick.AddListener(() => {
                manager.OpenDetailPopup(orderData);
            });
            Debug.Log("✅ Botón conectado correctamente.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error asignando el botón: " + ex.Message);
        }
    }
}
