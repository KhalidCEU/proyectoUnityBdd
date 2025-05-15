using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class CustomerManager : MonoBehaviour
{
    public GameObject addCustomerPanel;
    public GameObject buttonEliminar;
    public GameObject buttonEditar;
    public GameObject buttonGuardar;

    public Transform scrollContentContainer;
    public GameObject customerLinePrefab;

    public TMP_InputField searchInput;
    public Transform contentContainer;
    public GameObject customerItemPrefab;

    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
    public TMP_InputField phoneInput;
    public TMP_InputField addressInput;

    public GameObject detailPopup;
    public TMP_Text detailNameText;
    public TMP_Text detailEmailText;
    public TMP_Text detailPhoneText;
    public TMP_Text detailAddressText;

    public GameObject uiOutsidePopup;


    private bool isInAddMode = false;
    private bool isEditEnabled = false;

    private Customer selectedCustomer;
    private List<Customer> allCustomers = new List<Customer>();

    void Start()
    {
        LoadCustomers();
        searchInput.onValueChanged.AddListener(OnSearchChanged);
        searchInput.onSubmit.AddListener(delegate { OnSearchButtonClicked(); });
    }

    public void LoadCustomers()
    {
        allCustomers = DatabaseManager.GetAllCustomers();
        DisplayCustomers(allCustomers);
    }

    void DisplayCustomers(List<Customer> customers)
    {
        foreach (Transform child in contentContainer)
            Destroy(child.gameObject);

        foreach (Customer c in customers)
        {
            GameObject item = Instantiate(customerItemPrefab, contentContainer);
            item.GetComponent<CustomerItemUI>().Setup(c, this);
        }
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainView");
    }

    public void OnSearchChanged(string searchTerm)
    {
        string lowerTerm = searchTerm.ToLower();

        foreach (Transform child in scrollContentContainer)
        {
            TMP_Text text = child.GetComponentInChildren<TMP_Text>();

            if (text != null && text.text.ToLower().Contains(lowerTerm))
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void OnSearchButtonClicked()
    {
        OnSearchChanged(searchInput.text);
    }

    public void OpenAddCustomerPanel()
    {
        isInAddMode = true;
        isEditEnabled = false;

        addCustomerPanel.SetActive(true);
        uiOutsidePopup.SetActive(false);

        buttonEliminar.SetActive(false);
        buttonEditar.SetActive(false);
        buttonGuardar.SetActive(true);

        nameInput.text = "";
        emailInput.text = "";
        phoneInput.text = "";
        addressInput.text = "";
    }

    public void SaveNewCustomer()
    {
        Customer newCustomer = new Customer(
            nameInput.text,
            emailInput.text,
            phoneInput.text,
            addressInput.text
        );

        AddCustomerToScrollView(newCustomer);
        //DatabaseManager.AddCustomer(newCustomer);
        addCustomerPanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
    }

    private void AddCustomerToScrollView(Customer customer)
    {
        GameObject item = Instantiate(customerLinePrefab, scrollContentContainer);
        TMP_Text text = item.GetComponentInChildren<TMP_Text>();
        text.text = $"{customer.Id}. Name: {customer.Name}, Email: {customer.Email}";

        Button btn = item.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => {
                OpenDetailPopup(customer);
                OpenDetailPopupFromButton();
            });
        }
    }

    private Customer customerToShow;

    public void SetCustomerToShow(Customer c)
    {
        customerToShow = c;
    }

    public void OpenDetailPopupFromButton()
    {
        if (customerToShow != null)
        {
            OpenDetailPopup(customerToShow);
        }
    }

    public void OpenDetailPopup(Customer c)
    {
        Debug.Log("Opening popup for: " + c.Name);
        uiOutsidePopup.SetActive(false);
        isInAddMode = false;
        isEditEnabled = false;
        selectedCustomer = c;

        addCustomerPanel.SetActive(true);
        buttonEliminar.SetActive(true);
        buttonEditar.SetActive(true);
        buttonGuardar.SetActive(false);

        nameInput.text = c.Name;
        emailInput.text = c.Email;
        phoneInput.text = c.PhoneNumber;
        addressInput.text = c.Address;

        nameInput.interactable = false;
        emailInput.interactable = false;
        phoneInput.interactable = false;
        addressInput.interactable = false;
    }

    public void EditSelectedCustomer()
    {
        if (selectedCustomer == null) return;

        Customer updated = new Customer(
            selectedCustomer.Id,
            nameInput.text,
            emailInput.text,
            phoneInput.text,
            addressInput.text
        );

        DatabaseManager.UpdateCustomer(updated);
        ClosePopup();
        LoadCustomers();
    }

    public void HandleGuardar()
    {
        Debug.Log("Customer info saved");
        if (isInAddMode)
        {
            SaveNewCustomer();
        }
        else if (isEditEnabled)
        {
            EditSelectedCustomer();
        }
        else
        {
            Debug.LogWarning("Cannot save without edit mode.");
        }
    }

    public void EnableEditMode()
    {
        isEditEnabled = true;

        nameInput.interactable = true;
        emailInput.interactable = true;
        phoneInput.interactable = true;
        addressInput.interactable = true;

        buttonGuardar.SetActive(true);
    }

    public void DeleteSelectedCustomer()
    {
        DatabaseManager.DeleteCustomer(selectedCustomer.Id);
        ClosePopup();
        LoadCustomers();
    }

    public void ClosePopup()
    {
        addCustomerPanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
    }
}
