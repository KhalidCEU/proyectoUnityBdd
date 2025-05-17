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

        if (nameText != null)
        {
            nameText.text = $"{product.Id}. {product.Name}, Category: {product.CategoryId}";
        }

        Button btn = GetComponentInChildren<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => {
                manager.OpenDetailPopup(productData);
            });
        }
    }
}
