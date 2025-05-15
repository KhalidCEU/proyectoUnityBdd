using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductItemUI : MonoBehaviour
{
    private Product productData;
    private ProductManager manager;

    public TMP_Text nameText;

    public void Setup(Product product, ProductManager mgr)
    {
        productData = product;
        manager = mgr;

        nameText.text = product.Name;

        GetComponent<Button>().onClick.AddListener(() => {
            manager.OpenDetailPopup(productData);
        });
    }
}
