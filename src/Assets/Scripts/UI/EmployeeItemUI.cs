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

        nameText.text = employee.Name;

        GetComponent<Button>().onClick.AddListener(() => {
            manager.OpenDetailPopup(employeeData);
        });
    }
}

