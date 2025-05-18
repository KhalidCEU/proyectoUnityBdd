using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmployeeItemUI : MonoBehaviour
{
    private Employee employeeData;
    private EmployeeManager manager;

    public TMP_Text nameText;

    public void Setup(Employee employee, EmployeeManager mgr)
    {
        employeeData = employee;
        manager = mgr;

        if (nameText == null)
        {
            Debug.LogError("EmployeeItemUI: nameText reference is missing.");
            return;
        }

        nameText.text = $"{employee.Id}. {employee.Name}, {employee.Email}";

        var button = GetComponentInChildren<Button>();
        if (button == null)
        {
            Debug.LogError("EmployeeItemUI: Button reference is missing.");
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => manager.OpenDetailPopup(employeeData));
    }
}


