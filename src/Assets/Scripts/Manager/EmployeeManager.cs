using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class EmployeeManager : MonoBehaviour
{
    // Referencias del popup general (reutilizado)
    public GameObject addEmployeePanel;
    public GameObject buttonEliminar;
    public GameObject buttonEditar;
    public GameObject buttonGuardar;

    //para el scrollview
    public Transform scrollContentContainer; // donde instanciaras los prefabs e items 
    //public GameObject employeeLinePrefab;    // tu prefab tipo linea de texto



    // Referencias UI generales
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

    //este booleano sirve para diferenciar en que popup estamos para habilitar botones, en el de agregar o editar empleado directamente
    private bool isInAddMode = false;
    private bool isEditEnabled = false;

    private Employee selectedEmployee;
    private List<Employee> allEmployees = new List<Employee>(); //que cargue empelados en el prefab de la base de datos , nameText con metodo LoadEmployees y la lista AllEmployees

    private DbManager dbManager;

    void Start()
    {
        dbManager = FindFirstObjectByType<DbManager>(); //este es mas rapido y mejor para unity 
        LoadEmployees();
        searchInput.onValueChanged.AddListener(OnSearchChanged);
        searchInput.onSubmit.AddListener(delegate { OnSearchButtonClicked(); }); //para que el enter en dl input field de buscar funciona como la lupa
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

    foreach (Employee e in employees)
    {
        GameObject item = Instantiate(employeeItemPrefab, scrollContentContainer);
        item.GetComponent<EmployeeItemUI>().Setup(e, this);
        item.GetComponent<RectTransform>().localScale = Vector3.one; //pone el prefab dentro de la vista
        item.SetActive(true); //para que siempre se eva el prefab
    }
}

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainView");
    }

//para el boton input field del buscador
    public void OnSearchChanged(string searchTerm)
    {
    string lowerTerm = searchTerm.ToLower();

    foreach (Transform child in scrollContentContainer)
    {
        TMP_Text text = child.GetComponentInChildren<TMP_Text>();

        if (text != null && text.text.ToLower().Contains(lowerTerm))
        {
            child.gameObject.SetActive(true); // mostrar si coincide
        }
        else
        {
            child.gameObject.SetActive(false); // ocultar si no coincide
        }
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

        // Solo muestra los botones de cerrar y editar (guardar se muestra pero solo funcionara despues de darle a editar)
        buttonEliminar.SetActive(false);
        buttonEditar.SetActive(false);
        buttonGuardar.SetActive(true); // desactivado al principio

        // Limpiar campos
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
            Debug.LogWarning("Alguno de los campos numericos no tiene formato correcto.");
            return;
        }

        Employee newEmp = new Employee(
            nameInput.text,
            positionId,
            salary,
            emailInput.text,
            storeId
        );

        dbManager.AddEmployee(newEmp);                     // 1. Lo guardas
        allEmployees = dbManager.GetAllEmployees();        // 2. Lo recargas con ID real
        DisplayEmployees(allEmployees);                    // 3. Actualizas el scroll

    }


    // metodo para añadir a scroll el prefab con la infromacion de "Guardar"
    private void AddEmployeeToScrollView(Employee employee)
    {
        Debug.Log($"Anadiendo empleado al scroll: {employee.Name}");
        GameObject item = Instantiate(employeeItemPrefab, scrollContentContainer); //Crea una nueva copia del prefab en el scroll
        TMP_Text text = item.GetComponentInChildren<TMP_Text>(); //Busca el texto que muestra el empleado
        text.text = $"{employee.Id}. Name: {employee.Name}, Email: {employee.Email}"; //que solo muestre esto en la escena prinicpal de employees

        Button btn = item.GetComponent<Button>(); //Detecta si el prefab tiene un boton
        if (btn != null)
        {
            // boton para que el prefab sea clicable
            btn.onClick.AddListener(() => {
                OpenDetailPopup(employee);  //sale el popup con su infromacion
                
            });
        }
    }

    //este metodo y el siguiente son "trampas " para que el boton del prefab peuda llamar a OpenDetailPopup aun teniendo una referencia
    private Employee employeeToShow;

    public void SetEmployeeToShow(Employee e)
    {
        employeeToShow = e;
    }

    public void OpenDetailPopupFromButton()
    {
        if (employeeToShow != null)
        {
            OpenDetailPopup(employeeToShow);
        }
    }

    public void OpenDetailPopup(Employee e)
    {
        Debug.Log("Abriendo popup de " + e.Name);
        uiOutsidePopup.SetActive(false); //desativamos los demas botones que no esten en el popup
        isInAddMode = false;
        isEditEnabled = false;
        selectedEmployee = e;

        addEmployeePanel.SetActive(true);
        buttonEliminar.SetActive(true);
        buttonEditar.SetActive(true);
        buttonGuardar.SetActive(false); //hasta que se pulse el boton de editar

        // Rellenar campos
        nameInput.text = e.Name;
        positionIdInput.text = e.PositionId.ToString();
        salaryInput.text = e.Salary.ToString();
        emailInput.text = e.Email;
        storeIdInput.text = e.StoreId.ToString();

        // Desactivar edicion
        nameInput.interactable = false;
        positionIdInput.interactable = false;
        salaryInput.interactable = false;
        emailInput.interactable = false;
        storeIdInput.interactable = false;
    }

    //para editar los input fields
    public void EditSelectedEmployee()
    {
        if (selectedEmployee == null) return;

        // Limpiar salario (por si lleva simbolo €)
        string cleanSalary = salaryInput.text.Replace("€", "").Trim();
        float salaryParsed = float.Parse(cleanSalary);

        Employee updated = new Employee(
            selectedEmployee.Id,
            nameInput.text,
            int.Parse(positionIdInput.text), //poder escribir numeros en un input field
            salaryParsed,
            emailInput.text,
            selectedEmployee.Photo, // null de momento
            int.Parse(storeIdInput.text)
        );

        dbManager.UpdateEmployee(updated);
        ClosePopup();
        LoadEmployees();
    }

    public void HandleGuardar()
    {
        Debug.Log("info GUARDADA");
        if (isInAddMode)
        {
            SaveNewEmployee();
        }
        else if (isEditEnabled)
        {
            EditSelectedEmployee();
        }
        else
        {
            Debug.LogWarning("No puedes guardar sin activar el modo edicion.");
        }
    }

    //con este metodo los input fields de nuestro PopUp se podras activar y asi editarlos, cuando s ele da al boton editar
    public void EnableEditMode()
    {
        isEditEnabled = true;

        // Activar todos los campos para que ya se puedan rellenar
        nameInput.interactable = true;
        positionIdInput.interactable = true;
        salaryInput.interactable = true;
        emailInput.interactable = true;
        storeIdInput.interactable = true;

        //esto tiene sentido porque no s epeude guardar infromacion si no se ha editado nada
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
        uiOutsidePopup.SetActive(true); //activamos los botones
    }

}
