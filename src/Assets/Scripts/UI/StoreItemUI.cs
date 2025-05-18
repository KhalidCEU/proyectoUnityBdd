using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreItemUI : MonoBehaviour
{
    private Store storeData;
    private StoreManager manager;

    public TMP_Text nameText;

    public void Setup(Store store, StoreManager mgr)
    {
        storeData = store;
        manager = mgr;

        if (nameText == null)
        {
            Debug.LogError("StoreItemUI: nameText reference is missing.");
            return;
        }

        nameText.text = $"{store.Id}. {store.Address}";

        var button = GetComponentInChildren<Button>();
        if (button == null)
        {
            Debug.LogError("StoreItemUI: Button reference is missing.");
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => manager.OpenDetailPopup(storeData));
    }
}
