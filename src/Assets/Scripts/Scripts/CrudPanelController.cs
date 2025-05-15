using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CrudPanelController : MonoBehaviour
{
    private TMP_InputField searchInputField;

    void Start()
    {

        var header = transform.Find("SearchInputField");
        if (header == null) Debug.LogError("SearchInputField not found!");

        searchInputField = transform.Find("SearchInputField").GetComponent<TMP_InputField>();

        searchInputField.text = "";

        searchInputField.onValueChanged.AddListener(OnSearchValueChanged);
    }

    private void OnSearchValueChanged(string value)
    {
        Debug.Log("Search text: " + value);
        FilterList(value);
    }


    private void FilterList(string searchText)
    {
        Debug.Log("Filtering list with text: " + searchText);
    }
}
