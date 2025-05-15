using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreItemUI : MonoBehaviour
{
    private Store storeData;
    private StoreManager manager;

    public TMP_Text nameText;

    public void Setup(Store store, StoreManager mgr)
    {
        storeData = store;
        manager = mgr;

        nameText.text = $"{store.Id}. Address: {store.Address}";

        GetComponent<Button>().onClick.AddListener(() => {
            manager.SetStoreToShow(storeData);
            manager.OpenDetailPopupFromButton();
        });
    }
}
