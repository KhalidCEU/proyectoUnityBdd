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
        customerData = customer;
        manager = mgr;

        if (nameText == null)
        {
            Debug.LogError("CustomerItemUI: nameText reference is missing.");
            return;
        }

        nameText.text = $"{customer.Id}. {customer.Name}, {customer.Email}";

        var button = GetComponentInChildren<Button>();
        if (button == null)
        {
            Debug.LogError("CustomerItemUI: Button reference is missing.");
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => manager.OpenDetailPopup(customerData));
    }
}
