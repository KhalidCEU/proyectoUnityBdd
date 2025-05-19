using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreManager : MonoBehaviour
{
    public GameObject addStorePanel;
    public GameObject buttonEliminar;
    public GameObject buttonEditar;
    public GameObject buttonGuardar;

    public Transform scrollContentContainer;
    public TMP_InputField searchInput;
    public GameObject storeItemPrefab;

    public TMP_InputField addressInput;
    public TMP_InputField managerIdInput;

    public GameObject uiOutsidePopup;

    private bool isInAddMode = false;
    private bool isEditEnabled = false;

    private Store selectedStore;
    private List<Store> allStores = new List<Store>();
    private DbManager dbManager;

    void Start()
    {
        dbManager = FindFirstObjectByType<DbManager>();

        if (dbManager == null)
        {
            Debug.LogError("DbManager no encontrado en la escena.");
            return;
        }

        LoadStores();

        searchInput.onValueChanged.AddListener(OnSearchChanged);
        searchInput.onSubmit.AddListener(delegate { OnSearchButtonClicked(); });
    }

    public void LoadStores()
    {
        allStores = dbManager.GetAllStores();
        DisplayStores(allStores);
    }

    void DisplayStores(List<Store> stores)
    {
        foreach (Transform child in scrollContentContainer)
            Destroy(child.gameObject);

        foreach (Store s in stores)
        {
             GameObject item = Instantiate(storeItemPrefab, scrollContentContainer);
            item.GetComponent<StoreItemUI>().Setup(s, this);
            item.GetComponent<RectTransform>().localScale = Vector3.one;
            item.SetActive(true);
        }
    }

    private void AddStoreToScrollView(Store store)
    {
        Debug.Log($"Añadiendo tienda al scroll: {store.Address}");

        GameObject item = Instantiate(storeItemPrefab, scrollContentContainer);
        TMP_Text text = item.GetComponentInChildren<TMP_Text>();

        if (text != null)
        {
            text.text = $"{store.Id}. Address: {store.Address}";
            // Debug.Log("Texto asignado a prefab: " + text.text);
        }

        Button btn = item.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => {
                OpenDetailPopup(store);
            });
        }

        item.GetComponent<RectTransform>().localScale = Vector3.one;
        item.SetActive(true);
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

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainView");
    }

    public void OnSearchButtonClicked()
    {
        OnSearchChanged(searchInput.text);
    }

    public void OpenAddStorePanel()
    {
        isInAddMode = true;
        isEditEnabled = false;

        addStorePanel.SetActive(true);
        uiOutsidePopup.SetActive(false);

        buttonEliminar.SetActive(false);
        buttonEditar.SetActive(false);
        buttonGuardar.SetActive(true);

        addressInput.text = "";
        managerIdInput.text = "";
    }

    public void SaveNewStore()
    {
        int managerId;

        if (!int.TryParse(managerIdInput.text, out managerId))
        {
            Debug.LogWarning("Manager ID inválido.");
            return;
        }

        Store newStore = new Store(addressInput.text, managerId);

        dbManager.AddStore(newStore);
        allStores = dbManager.GetAllStores(); // 💡 Recarga con IDs correctos
        DisplayStores(allStores);            // 🔄 Refresca la vista

        addStorePanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
    }

    public void OpenDetailPopup(Store s)
    {
        uiOutsidePopup.SetActive(false);
        isInAddMode = false;
        isEditEnabled = false;
        selectedStore = s;

        addStorePanel.SetActive(true);
        buttonEliminar.SetActive(true);
        buttonEditar.SetActive(true);
        buttonGuardar.SetActive(false);

        addressInput.text = s.Address;
        managerIdInput.text = s.ManagerId.ToString();

        addressInput.interactable = false;
        managerIdInput.interactable = false;
    }

    public void EditSelectedStore()
    {
        if (selectedStore == null) return;

        Store updated = new Store(
            selectedStore.Id,
            addressInput.text,
            int.Parse(managerIdInput.text)
        );

        dbManager.UpdateStore(updated);
        ClosePopup();
        LoadStores();
    }

    public void HandleGuardar()
    {
        if (isInAddMode)
        {
            Debug.Log("Añadiendo nueva tienda...");
            SaveNewStore();
        }
            
        else if (isEditEnabled)
        {
            Debug.Log("Editando tienda existente...");
            EditSelectedStore();
        }
            
        else
        {
            Debug.LogWarning("No puedes guardar sin activar el modo edición.");
        }
            
    }

    public void EnableEditMode()
    {
        isEditEnabled = true;
        isInAddMode = false;
        addressInput.interactable = true;
        managerIdInput.interactable = true;
        buttonGuardar.SetActive(true);
    }

    public void DeleteSelectedStore()
    {
        dbManager.DeleteStore(selectedStore.Id);
        ClosePopup();
        LoadStores();
    }

    public void ClosePopup()
    {
        addStorePanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
    }
}
