
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainUI : MonoBehaviour
{
    public Button myButton;
    public TextMeshProUGUI outputText;

    void Start()
    {
        myButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        outputText.text = "Hello World !";
    }
}
