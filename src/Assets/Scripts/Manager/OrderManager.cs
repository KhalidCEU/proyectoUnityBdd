using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections;



public class OrderManager : MonoBehaviour
{
    
    // Referencias del popup general (reutilizado)
    public GameObject addOrderPanel;
    public GameObject buttonEliminar;
    public GameObject buttonEditar;
    public GameObject buttonGuardar;

    // ScrollView
    public Transform scrollContentContainer;
    //public GameObject orderLinePrefab;

    // Referencias UI generales
    public TMP_InputField searchInput;
    //public Transform contentContainer;
    public GameObject orderItemPrefab;

    // Campos del formulario del popup
    public TMP_InputField dateInput;
    public TMP_InputField customerIdInput;
    public TMP_InputField totalAmountInput;

    // Detalle popup (si lo usas aparte)
    public GameObject detailPopup;
    public TMP_Text detailDateText;
    public TMP_Text detailCustomerIdText;
    public TMP_Text detailAmountText;

    public GameObject uiOutsidePopup;

    private bool isInAddMode = false;
    private bool isEditEnabled = false;

    private Order selectedOrder;
    private List<Order> allOrders = new List<Order>();
    
    private DbManager dbManager;

void Start()
{
    dbManager = FindFirstObjectByType<DbManager>();
    if (dbManager == null)
    {
        Debug.LogError("DbManager no encontrado en la escena. Asegúrate de que hay un GameObject con el script DbManager activo.");
        return;
    }

    LoadOrders(); 
    searchInput.onValueChanged.AddListener(OnSearchChanged);
    searchInput.onSubmit.AddListener(delegate { OnSearchButtonClicked(); });
}


    public void LoadOrders()
    {
        allOrders = dbManager.GetAllOrders();
        DisplayOrders(allOrders);
    }

   void DisplayOrders(List<Order> orders)
{
    Debug.Log("DisplayOrders llamado con " + orders.Count + " órdenes");

    foreach (Transform child in scrollContentContainer)
        Destroy(child.gameObject);

    foreach (Order o in orders)
    {
        GameObject item = Instantiate(orderItemPrefab, scrollContentContainer);
        item.GetComponent<OrderItemUI>().Setup(o, this);
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

    public void OpenAddOrderPanel()
    {
        isInAddMode = true;
        isEditEnabled = false;

        addOrderPanel.SetActive(true);
        uiOutsidePopup.SetActive(false);

        buttonEliminar.SetActive(false);
        buttonEditar.SetActive(false);
        buttonGuardar.SetActive(true);

        dateInput.text = "";
        customerIdInput.text = "";
        totalAmountInput.text = "";
    }

    public void SaveNewOrder()
{
    int customerId;
    float totalAmount;

    if (!int.TryParse(customerIdInput.text, out customerId) ||
        !float.TryParse(totalAmountInput.text, out totalAmount))
    {
        Debug.LogWarning("One or more numeric fields are not in correct format.");
        return;
    }

    if (totalAmount < 0)
    {
        Debug.LogWarning("Total amount cannot be negative.");
        return;
    }

    Order newOrder = new Order(dateInput.text, customerId, totalAmount);
Debug.Log("dbManager: " + dbManager);
Debug.Log(" dateInput: " + dateInput);
Debug.Log(" customerIdInput: " + customerIdInput);
Debug.Log(" totalAmountInput: " + totalAmountInput);


    dbManager.AddOrder(newOrder);
    allOrders = dbManager.GetAllOrders(); // actualizamos con IDs reales
    DisplayOrders(allOrders);

    addOrderPanel.SetActive(false);
    uiOutsidePopup.SetActive(true);
}

    private void AddOrderToScrollView(Order order)
    {
        Debug.Log($"Anadiendo order al scroll: {order.Id}");
        GameObject item = Instantiate(orderItemPrefab, scrollContentContainer);
        TMP_Text text = item.GetComponentInChildren<TMP_Text>();
        text.text = $"{order.Id}. Date: {order.Date}, Customer Id: {order.CustomerId}";

        Button btn = item.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => {
                SetOrderToShow(order);
                
            });
        }
    }

    private Order orderToShow;

    public void SetOrderToShow(Order o)
    {
        orderToShow = o;
    }

    public void OpenDetailPopupFromButton()
    {
        if (orderToShow != null)
        {
            OpenDetailPopup(orderToShow);
        }
    }

    public void OpenDetailPopup(Order o)
    {
        uiOutsidePopup.SetActive(false);
        isInAddMode = false;
        isEditEnabled = false;
        selectedOrder = o;

        addOrderPanel.SetActive(true);
        buttonEliminar.SetActive(true);
        buttonEditar.SetActive(true);
        buttonGuardar.SetActive(false);

        dateInput.text = o.Date;
        customerIdInput.text = o.CustomerId.ToString();
        totalAmountInput.text = o.TotalAmount.ToString();

        dateInput.interactable = false;
        customerIdInput.interactable = false;
        totalAmountInput.interactable = false;
    }

    public void EditSelectedOrder()
    {
        if (selectedOrder == null) return;

        string cleanAmount = totalAmountInput.text.Replace("$", "").Trim();
        float parsedAmount = float.Parse(cleanAmount);

        Order updated = new Order(
            selectedOrder.Id,
            dateInput.text,
            int.Parse(customerIdInput.text),
            parsedAmount
        );

        dbManager.UpdateOrder(updated);
        ClosePopup();
        LoadOrders();
    }

    public void HandleGuardar()
    {
        if (isInAddMode)
        {
            SaveNewOrder();
        }
        else if (isEditEnabled)
        {
            EditSelectedOrder();
        }
        else
        {
            Debug.LogWarning("You must activate edit mode before saving.");
        }
    }

    public void EnableEditMode()
    {
        isEditEnabled = true;

        dateInput.interactable = true;
        customerIdInput.interactable = true;
        totalAmountInput.interactable = true;

        buttonGuardar.SetActive(true);
    }

    public void DeleteSelectedOrder()
    {
        dbManager.DeleteOrder(selectedOrder.Id);
        ClosePopup();
        LoadOrders();
    }

    public void ClosePopup()
    {
        addOrderPanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
    }
}
