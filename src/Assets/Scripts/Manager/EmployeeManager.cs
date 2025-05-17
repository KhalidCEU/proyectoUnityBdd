using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class EmployeeManager : MonoBehaviour
{
    public GameObject addEmployeePanel;
    public GameObject buttonEliminar;
    public GameObject buttonEditar;
    public GameObject buttonGuardar;

    public Transform scrollContentContainer;
    public TMP_InputField searchInput;
    public GameObject employeeItemPrefab;

    // Campos del formulario del popup
    public TMP_InputField nameInput;
    public TMP_InputField positionIdInput;
    public TMP_InputField salaryInput;
    public TMP_InputField emailInput;
    public TMP_InputField storeIdInput;

    // nombres del popup de "detalles" que se abre, que es basicamente cuando pinchas en el prefeb pero no añades un nuevo empleado
    public GameObject detailPopup;
    public TMP_Text detailNameText;
    public TMP_Text detailEmailText;
    public TMP_Text detailPositionText;
    public TMP_Text detailSalaryText;
    public TMP_Text detailStoreText;

    //desactivar botones de la escena principal cuando se abre el popup
    public GameObject uiOutsidePopup;

    // Booleans para diferenciar en que popup estamos (añadir/editar)
    private bool isInAddMode = false;
    private bool isEditEnabled = false;

    private Employee selectedEmployee;
    private List<Employee> allEmployees = new List<Employee>();
    private DbManager dbManager;

    void Start()
    {
        dbManager = FindFirstObjectByType<DbManager>();
        LoadEmployees();
        searchInput.onValueChanged.AddListener(OnSearchChanged);
        searchInput.onSubmit.AddListener(delegate { OnSearchButtonClicked(); });
    }

    public void LoadEmployees()
    {
        allEmployees = dbManager.GetAllEmployees();
        DisplayEmployees(allEmployees);
    }

    void DisplayEmployees(List<Employee> employees)
    {
        foreach (Transform child in scrollContentContainer)
            Destroy(child.gameObject);

        foreach (Employee emp in employees)
        {
            GameObject item = Instantiate(employeeItemPrefab, scrollContentContainer);
            item.GetComponent<EmployeeItemUI>().Setup(emp, this);
            item.GetComponent<RectTransform>().localScale = Vector3.one;
            item.SetActive(true);
        }
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainView");
    }

    public void OnSearchChanged(string searchTerm)
    {
        string lowerTerm = searchTerm.ToLower();

        foreach (Transform child in scrollContentContainer)
        {
            TMP_Text text = child.GetComponentInChildren<TMP_Text>();
            if (text != null && text.text.ToLower().Contains(lowerTerm))
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }
    }

    public void OnSearchButtonClicked()
    {
        OnSearchChanged(searchInput.text);
    }

    public void OpenAddEmployeePanel()
    {
        isInAddMode = true;
        isEditEnabled = false;

        addEmployeePanel.SetActive(true);
        uiOutsidePopup.SetActive(false);

        buttonEliminar.SetActive(false);
        buttonEditar.SetActive(false);
        buttonGuardar.SetActive(true);

        nameInput.text = "";
        positionIdInput.text = "";
        salaryInput.text = "";
        emailInput.text = "";
        storeIdInput.text = "";
    }

    public void SaveNewEmployee()
    {
        int positionId, storeId;
        float salary;

        if (!int.TryParse(positionIdInput.text, out positionId) ||
            !float.TryParse(salaryInput.text, out salary) ||
            !int.TryParse(storeIdInput.text, out storeId))
        {
            Debug.LogWarning("ERROR: Invalid format in one of the fields.");
            return;
        }

        Employee newEmployee = new Employee(
            nameInput.text,
            positionId,
            salary,
            emailInput.text,
            storeId
        );

        dbManager.AddEmployee(newEmployee);
        allEmployees = dbManager.GetAllEmployees();
        DisplayEmployees(allEmployees);

        addEmployeePanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
    }

    public void OpenDetailPopup(Employee emp)
    {
        Debug.Log("Opening popup for: " + emp.Name);
        uiOutsidePopup.SetActive(false);
        isInAddMode = false;
        isEditEnabled = false;
        selectedEmployee = emp;

        addEmployeePanel.SetActive(true);
        buttonEliminar.SetActive(true);
        buttonEditar.SetActive(true);
        buttonGuardar.SetActive(false);

        nameInput.text = emp.Name;
        positionIdInput.text = emp.PositionId.ToString();
        salaryInput.text = emp.Salary.ToString();
        emailInput.text = emp.Email;
        storeIdInput.text = emp.StoreId.ToString();

        nameInput.interactable = false;
        positionIdInput.interactable = false;
        salaryInput.interactable = false;
        emailInput.interactable = false;
        storeIdInput.interactable = false;
    }

    public void EditSelectedEmployee()
    {
        if (selectedEmployee == null) return;

        int positionId, storeId;
        float salary;

        if (!int.TryParse(positionIdInput.text, out positionId) ||
            !float.TryParse(salaryInput.text, out salary) ||
            !int.TryParse(storeIdInput.text, out storeId))
        {
            Debug.Log("ERROR: Invalid format in one of the fields.");
            return;
        }

        Employee updated = new Employee(
            nameInput.text,
            positionId,
            salary,
            emailInput.text,
            storeId
        );

        dbManager.UpdateEmployee(updated);
        ClosePopup();
        LoadEmployees();
    }

    public void HandleGuardar()
    {
        if (isEditEnabled)
        {
            Debug.Log("Editing employee");
            EditSelectedEmployee();
        }

        Debug.Log("Saving employee");
        SaveNewEmployee();
    }

    public void EnableEditMode()
    {
        isEditEnabled = true;

        nameInput.interactable = true;
        positionIdInput.interactable = true;
        salaryInput.interactable = true;
        emailInput.interactable = true;
        storeIdInput.interactable = true;

        buttonGuardar.SetActive(true);
    }

    public void DeleteSelectedEmployee()
    {
        dbManager.DeleteEmployee(selectedEmployee.Id);
        ClosePopup();
        LoadEmployees();
    }

    public void ClosePopup()
    {
        addEmployeePanel.SetActive(false);
        uiOutsidePopup.SetActive(true);
    }
}
