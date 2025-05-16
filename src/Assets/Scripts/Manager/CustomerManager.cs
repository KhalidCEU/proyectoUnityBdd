using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomerManager : MonoBehaviour
{
    public GameObject addCustomerPanel;
    public GameObject buttonEliminar;
    public GameObject buttonEditar;
    public GameObject buttonGuardar;

    public Transform scrollContentContainer;
    public TMP_InputField searchInput;
    public GameObject customerItemPrefab;

    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
    public TMP_InputField phoneInput;
    public TMP_InputField addressInput;

    public GameObject uiOutsidePopup;

    private bool isInAddMode = false;
    private bool isEditEnabled = false;

    private Customer selectedCustomer;
    private List<Customer> allCustomers = new List<Customer>();
    private DbManager dbManager;

    void Start()
    {
        dbManager = FindFirstObjectByType<DbManager>(); //esyo es mas rapido para unity 

        LoadCustomers();
        searchInput.onValueChanged.AddListener(OnSearchChanged);
        searchInput.onSubmit.AddListener(delegate { OnSearchButtonClicked(); });
    }

    public void LoadCustomers()
    {
        allCustomers = dbManager.GetAllCustomers();
        DisplayCustomers(allCustomers);
    }

    void DisplayCustomers(List<Customer> customers)
    {
        foreach (Transform child in scrollContentContainer)
            Destroy(child.gameObject);

        foreach (Customer c in customers)
        {
            GameObject item = Instantiate(customerItemPrefab, scrollContentContainer);
            item.GetComponent<CustomerItemUI>().Setup(c, this);
            item.GetComponent<RectTransform>().localScale = Vector3.one;
            item.SetActive(true);
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
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
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

        dbManager.AddCustomer(newCustomer);
        allCustomers = dbManager.GetAllCustomers();
        DisplayCustomers(allCustomers);

        addCustomerPanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
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

        dbManager.UpdateCustomer(updated);
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
        dbManager.DeleteCustomer(selectedCustomer.Id);
        ClosePopup();
        LoadCustomers();
    }

    public void ClosePopup()
    {
        addCustomerPanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
    }
}
