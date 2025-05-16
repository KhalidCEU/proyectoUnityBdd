
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
        Debug.Log(" Setup() iniciado para: " + (employee != null ? employee.Name : "employee NULL"));

        employeeData = employee;
        manager = mgr;

        if (nameText == null)
        {
            Debug.LogError(" nameText está NULL en Setup()");
        }
        else
        {
            Debug.Log(" nameText no es null");
        }

        try
        {
            nameText.text = $"{employee.Id}. {employee.Name}, {employee.Email}";
            Debug.Log(" Texto asignado: " + nameText.text);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(" Error al asignar texto: " + ex.Message);
        }

        try
        {
            GetComponent<Button>().onClick.AddListener(() => {
                manager.OpenDetailPopup(employeeData);
            });
            Debug.Log(" Botón conectado correctamente.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error asignando el botón: " + ex.Message);
        }
    }
}


