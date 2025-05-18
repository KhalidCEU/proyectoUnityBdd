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
        orderData = order;
        manager = mgr;

        if (nameText == null)
        {
            Debug.LogError("OrderItemUI: nameText reference is missing.");
            return;
        }

        nameText.text = $"{order.Id}. Date: {order.Date} | Customer ID: {order.CustomerId}";

        var button = GetComponentInChildren<Button>();
        if (button == null)
        {
            Debug.LogError("OrderItemUI: Button reference is missing.");
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => manager.OpenDetailPopup(orderData));
    }
}
