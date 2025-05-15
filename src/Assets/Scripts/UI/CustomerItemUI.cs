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

        nameText.text = customer.Name;

        GetComponent<Button>().onClick.AddListener(() => {
            manager.SetCustomerToShow(customerData);
            manager.OpenDetailPopupFromButton();
        });
    }
}
