using UnityEngine;
using TMPro; // For TMP Text
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class EmployeeSceneManager : MonoBehaviour
{
    public GameObject crudPanelPrefab;
    public Transform scrollViewContent;
    public GameObject CrudPanel;

    private IDbConnection dbConnection;

    void Start()
    {
        dbConnection = DBManager.GetDbConnection();

        //TMP_Text displayText = CrudPanel.transform.Find("SearchInputField").GetComponent<TMP_Text>();
        //displayText.text = "Hello, this is your data!";

        // Load employees from the database
        LoadEmployees();
    }


    private void LoadEmployees()
    {
        string query = "SELECT * FROM Employees";

        using (var command = dbConnection.CreateCommand())
        {
            command.CommandText = query;
            using (var reader = command.ExecuteReader())
            {
                // Clear previous content from the scroll view
                foreach (Transform child in scrollViewContent)
                {
                    Destroy(child.gameObject);
                }

                while (reader.Read())
                {
                    string name = reader.GetString(1);  // 'name' is the second column
                    int positionId = reader.GetInt32(2);  // 'positionId' is the third column

                    // Create a new GameObject for each employee
                    GameObject employeeItem = new GameObject(name);

                    // Add RectTransform for layout control
                    RectTransform rectTransform = employeeItem.AddComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(200, 50);  // Set a reasonable size for the item

                    // Add a TextMeshProUGUI component for displaying the employee's info
                    TextMeshProUGUI employeeText = employeeItem.AddComponent<TextMeshProUGUI>();
                    employeeText.text = $"Name: {name}\nPositionId: {positionId}";
                    employeeText.fontSize = 12;  // Adjust the font size to something more reasonable

                    // Set text alignment for better readability
                    employeeText.alignment = TextAlignmentOptions.Left;  // Left-aligned text
                    employeeText.verticalAlignment = VerticalAlignmentOptions.Middle;  // Vertically center the text
                    employeeText.enableAutoSizing = true;  // Enable auto-sizing for better text fitting

                    // Optionally add a Button if you want to handle clicks
                    Button employeeButton = employeeItem.AddComponent<Button>();
                    employeeButton.onClick.AddListener(() => OnEmployeeClick(name, positionId));

                    // Parent the employee item to the ScrollView Content
                    employeeItem.transform.SetParent(scrollViewContent, false);  // "false" keeps the local position intact

                    // Add a LayoutElement for better control
                    LayoutElement layoutElement = employeeItem.AddComponent<LayoutElement>();
                    layoutElement.preferredHeight = 50;  // Set the height of each item

                    // Optionally set a minimum width and height to ensure everything fits well
                    layoutElement.minWidth = 200;
                    layoutElement.minHeight = 50;
                }
            }

        }
    }

    private void OnEmployeeClick(string name, int positionId)
    {
        Debug.Log($"Employee {name} clicked. Position: {positionId}");
    }
}
