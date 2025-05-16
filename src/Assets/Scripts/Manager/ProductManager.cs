using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ProductManager : MonoBehaviour
{
    public GameObject addProductPanel;
    public GameObject buttonEliminar;
    public GameObject buttonEditar;
    public GameObject buttonGuardar;

    public Transform scrollContentContainer;
    //public GameObject productLinePrefab;


    public TMP_InputField searchInput;
    //public Transform contentContainer;
    public GameObject productItemPrefab;

    public TMP_InputField nameInput;
    public TMP_InputField categoryIdInput;
    public TMP_InputField priceInput;
    public TMP_InputField stockInput;

    public GameObject uiOutsidePopup;

    private bool isInAddMode = false;
    private bool isEditEnabled = false;

    private Product selectedProduct;
    private List<Product> allProducts = new List<Product>();

    void Start()
    {
        LoadProducts();
        searchInput.onValueChanged.AddListener(OnSearchChanged);
        searchInput.onSubmit.AddListener(delegate { OnSearchButtonClicked(); });
    }

    public void LoadProducts()
    {
        allProducts = DatabaseManager.GetAllProducts();
        DisplayProducts(allProducts);
    }

    void DisplayProducts(List<Product> products)
    {
        foreach (Transform child in scrollContentContainer)
            Destroy(child.gameObject);

        foreach (Product p in products)
        {
            GameObject item = Instantiate(productItemPrefab, scrollContentContainer);
            item.GetComponent<ProductItemUI>().Setup(p, this);
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

    public void OpenAddProductPanel()
    {
        isInAddMode = true;
        isEditEnabled = false;
        addProductPanel.SetActive(true);
        uiOutsidePopup.SetActive(false);

        buttonEliminar.SetActive(false);
        buttonEditar.SetActive(false);
        buttonGuardar.SetActive(true);

        nameInput.text = "";
        categoryIdInput.text = "";
        priceInput.text = "";
        stockInput.text = "";
    }

    public void SaveNewProduct()
    {
        int categoryId, stock;
        float price;

        if (!int.TryParse(categoryIdInput.text, out categoryId) ||
            !float.TryParse(priceInput.text, out price) ||
            !int.TryParse(stockInput.text, out stock))
        {
            Debug.LogWarning("Invalid format in numerical fields.");
            return;
        }

        Product newProduct = new Product(nameInput.text, categoryId, price, stock);

        AddProductToScrollView(newProduct);
        DatabaseManager.AddProduct(newProduct);

        addProductPanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
    }

    private void AddProductToScrollView(Product product)
    {
        GameObject item = Instantiate(productItemPrefab, scrollContentContainer);
        TMP_Text text = item.GetComponentInChildren<TMP_Text>();
        text.text = $"{product.Id}. {product.Name}, Category: {product.CategoryId}";

        Button btn = item.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => {
                OpenDetailPopup(product);
                OpenDetailPopupFromButton();
            });
        }
    }

    private Product productToShow;

    public void SetProductToShow(Product p)
    {
        productToShow = p;
    }

    public void OpenDetailPopupFromButton()
    {
        if (productToShow != null)
        {
            OpenDetailPopup(productToShow);
        }
    }

    public void OpenDetailPopup(Product p)
    {
        uiOutsidePopup.SetActive(false);
        isInAddMode = false;
        isEditEnabled = false;
        selectedProduct = p;

        addProductPanel.SetActive(true);
        buttonEliminar.SetActive(true);
        buttonEditar.SetActive(true);
        buttonGuardar.SetActive(false);

        nameInput.text = p.Name;
        categoryIdInput.text = p.CategoryId.ToString();
        priceInput.text = p.Price.ToString();
        stockInput.text = p.Stock.ToString();

        nameInput.interactable = false;
        categoryIdInput.interactable = false;
        priceInput.interactable = false;
        stockInput.interactable = false;
    }

    public void EditSelectedProduct()
    {
        if (selectedProduct == null) return;

        float cleanPrice = float.Parse(priceInput.text);

        Product updated = new Product(
            selectedProduct.Id,
            nameInput.text,
            int.Parse(categoryIdInput.text),
            cleanPrice,
            int.Parse(stockInput.text)
        );

        DatabaseManager.UpdateProduct(updated);
        ClosePopup();
        LoadProducts();
    }

    public void HandleGuardar()
    {
        if (isInAddMode)
            SaveNewProduct();
        else if (isEditEnabled)
            EditSelectedProduct();
        else
            Debug.LogWarning("Cannot save without edit mode.");
    }

    public void EnableEditMode()
    {
        isEditEnabled = true;

        nameInput.interactable = true;
        categoryIdInput.interactable = true;
        priceInput.interactable = true;
        stockInput.interactable = true;

        buttonGuardar.SetActive(true);
    }

    public void DeleteSelectedProduct()
    {
        DatabaseManager.DeleteProduct(selectedProduct.Id);
        ClosePopup();
        LoadProducts();
    }

    public void ClosePopup()
    {
        addProductPanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
    }
}
