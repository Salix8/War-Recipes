using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

[RequireComponent(typeof(Button))] // Asegura que haya un componente Button en el GameObject
public class SceneButton : MonoBehaviour
{
    [Tooltip("The scene to load when the button is clicked.")]
    [SerializeField] private string sceneToLoad;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(LoadSelectedScene);
        }
        else
        {
            Debug.LogError("Button component not found! SceneButton()");
        }
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(LoadSelectedScene);
    }

    public void LoadSelectedScene()
    {
        // Utiliza GameManager.Instance directamente
        if (GameManager.Instance != null && !string.IsNullOrEmpty(sceneToLoad))
        {
            GameManager.Instance.ChangeScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("GameManager.Instance is null or no scene selected! Asegúrate de que el GameManager se haya inicializado correctamente. SceneButton()");
        }
    }


#if UNITY_EDITOR
    // Custom Editor for the SceneButton script
    [CustomEditor(typeof(SceneButton))]
    public class SceneButtonEditor : Editor
    {
        private SerializedProperty sceneToLoadProperty;

        private void OnEnable()
        {
            sceneToLoadProperty = serializedObject.FindProperty("sceneToLoad");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw the default inspector fields
            //DrawDefaultInspector(); // Quita esto para controlar completamente el Inspector

            // Get the SceneButton instance
            SceneButton sceneButton = (SceneButton)target;

            // Usar EditorGUILayout.PropertyField para el Tooltip
            EditorGUILayout.PropertyField(sceneToLoadProperty, new GUIContent("Scene To Load"));

            // Ensure GameManager and SceneListSO are assigned
            if (GameManager.Instance != null && GameManager.Instance.sceneList != null)
            {
                // Get the list of scene names from the SceneListSO
                List<string> sceneNames = GameManager.Instance.sceneList.sceneNames;

                // Find the index of the currently selected scene
                int selectedIndex = sceneNames.IndexOf(sceneToLoadProperty.stringValue);

                // Create a popup (dropdown) with the scene names
                selectedIndex = EditorGUILayout.Popup("Scene To Load", selectedIndex, sceneNames.ToArray());

                // If a new scene was selected, update the sceneToLoad property
                if (selectedIndex >= 0 && selectedIndex < sceneNames.Count)
                {
                    sceneToLoadProperty.stringValue = sceneNames[selectedIndex];
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Asegúrate de que GameManager.Instance se haya inicializado correctamente y que SceneListSO esté asignado en el GameManager.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}

/*
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

[RequireComponent(typeof(Button))] // Asegura que haya un componente Button en el GameObject
public class SceneButton : MonoBehaviour
{
[Tooltip("The GameManager containing the list of scenes.")]
public GameManager gameManager;

[Tooltip("The scene to load when the button is clicked.")]
[SerializeField] private string sceneToLoad;

private Button button;

void Start()
{
button = GetComponent<Button>();
if (button != null)
{
    button.onClick.AddListener(LoadSelectedScene);
}
else
{
    Debug.LogError("Button component not found! SceneButton()");
}
}

void OnDestroy()
{
button.onClick.RemoveListener(LoadSelectedScene);
}

public void LoadSelectedScene()
{
if (gameManager != null && !string.IsNullOrEmpty(sceneToLoad))
{
    gameManager.ChangeScene(sceneToLoad);
}
else
{
    Debug.LogError("GameManager not assigned or no scene selected!");
}
}
#if UNITY_EDITOR
// Custom Editor for the SceneButton script
[CustomEditor(typeof(SceneButton))]
public class SceneButtonEditor : Editor
{
private SerializedProperty sceneToLoadProperty;

private void OnEnable()
    {
        sceneToLoadProperty = serializedObject.FindProperty("sceneToLoad");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw the default inspector fields
        DrawDefaultInspector();

        // Get the SceneButton instance
        SceneButton sceneButton = (SceneButton)target;

        // Ensure GameManager and SceneListSO are assigned
        if (sceneButton.gameManager != null && sceneButton.gameManager.sceneList != null)
        {
            // Get the list of scene names from the SceneListSO
            List<string> sceneNames = sceneButton.gameManager.sceneList.sceneNames;

            // Find the index of the currently selected scene
            int selectedIndex = sceneNames.IndexOf(sceneToLoadProperty.stringValue);

            // Create a popup (dropdown) with the scene names
            selectedIndex = EditorGUILayout.Popup("Scene To Load", selectedIndex, sceneNames.ToArray());

            // If a new scene was selected, update the sceneToLoad property
            if (selectedIndex >= 0 && selectedIndex < sceneNames.Count)
            {
                sceneToLoadProperty.stringValue = sceneNames[selectedIndex];
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Assign a GameManager with a valid SceneListSO to see the scene selection dropdown.", MessageType.Warning);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
}
*/