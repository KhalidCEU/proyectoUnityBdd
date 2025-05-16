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
        Debug.Log("Setup() iniciado para: " + (store != null ? store.Address : "store NULL"));

        storeData = store;
        manager = mgr;

        if (nameText == null)
        {
            Debug.LogError("nameText está NULL en Setup()");
        }
        else
        {
            Debug.Log("nameText no es null");
        }

        try
        {
            nameText.text = $"{store.Id}. {store.Address}";
            Debug.Log("Texto asignado: " + nameText.text);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al asignar texto: " + ex.Message);
        }

        try
        {
            GetComponent<Button>().onClick.AddListener(() => {
                manager.OpenDetailPopup(storeData);
            });
            Debug.Log("Botón conectado correctamente.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error asignando el botón: " + ex.Message);
        }
    }
}
