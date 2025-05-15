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
    public GameObject storeLinePrefab;

    public TMP_InputField searchInput;
    public Transform contentContainer;
    public GameObject storeItemPrefab;

    public TMP_InputField addressInput;
    public TMP_InputField managerIdInput;

    public GameObject uiOutsidePopup;

    private bool isInAddMode = false;
    private bool isEditEnabled = false;

    private Store selectedStore;
    private List<Store> allStores = new List<Store>();

    void Start()
    {
        LoadStores();
        searchInput.onValueChanged.AddListener(OnSearchChanged);
        searchInput.onSubmit.AddListener(delegate { OnSearchButtonClicked(); });
    }

    public void LoadStores()
    {
        allStores = DatabaseManager.GetAllStores();
        DisplayStores(allStores);

    }

    void DisplayStores(List<Store> stores)
    {
        foreach (Transform child in contentContainer)
            Destroy(child.gameObject);

        foreach (Store s in stores)
        {
            GameObject item = Instantiate(storeItemPrefab, contentContainer);
            item.GetComponent<StoreItemUI>().Setup(s, this);
        }
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
        SceneManager.LoadScene("Principal 1");
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
            Debug.LogWarning("Invalid manager ID.");
            return;
        }

        Store newStore = new Store(addressInput.text, managerId);

        AddStoreToScrollView(newStore);
        //DatabaseManager.AddStore(newStore);
        addStorePanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
    }

    private void AddStoreToScrollView(Store store)
    {
        GameObject item = Instantiate(storeLinePrefab, scrollContentContainer);
        TMP_Text text = item.GetComponentInChildren<TMP_Text>();
        text.text = $"{store.Id}. Address: {store.Address}";

        Button btn = item.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => {
                OpenDetailPopup(store);
                //OpenDetailPopupFromButton();
            });
        }
    }

    private Store storeToShow;

    public void SetStoreToShow(Store s)
    {
        storeToShow = s;
    }

    public void OpenDetailPopupFromButton()
    {
        if (storeToShow != null)
            OpenDetailPopup(storeToShow);
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

        DatabaseManager.UpdateStore(updated);
        ClosePopup();
        LoadStores();
    }

    public void HandleGuardar()
    {
        if (isInAddMode)
            SaveNewStore();
        else if (isEditEnabled)
            EditSelectedStore();
        else
            Debug.LogWarning("Editing not enabled");
    }

    public void EnableEditMode()
    {
        isEditEnabled = true;
        addressInput.interactable = true;
        managerIdInput.interactable = true;
        buttonGuardar.SetActive(true);
    }

    public void DeleteSelectedStore()
    {
        DatabaseManager.DeleteStore(selectedStore.Id);
        ClosePopup();
        LoadStores();
    }

    public void ClosePopup()
    {
        addStorePanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
    }
}
