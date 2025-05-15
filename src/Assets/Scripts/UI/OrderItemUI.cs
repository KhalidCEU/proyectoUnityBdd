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

        nameText.text = $"{order.Id}. Date: {order.Date}  Customer Id: {order.CustomerId}";

        GetComponent<Button>().onClick.AddListener(() => {
            manager.SetOrderToShow(orderData); // Truco para no perder el parámetro
            manager.OpenDetailPopupFromButton();
        });
    }
}
