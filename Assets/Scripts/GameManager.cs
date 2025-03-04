using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Scene Management")]
    [Tooltip("ScriptableObject containing the list of scene names.")]
    public SceneListSO sceneList; // Assign this in the Inspector!

    [Tooltip("Maximum number of scenes allowed in the history stack.")]
    public int maxScenesInStack = 5; //No hacen falta mas: MainMenu > Faccion > Mapa1-3 > Restaurante1-3 > Combate1-3

    [Header("Debug")]
    [Tooltip("Enable to log scene changes and stack status to the console.")]
    public bool enableDebugLogs = false;

    // Datos persistentes:
    //public Dictionary<string, int> ingredients = new Dictionary<string, int>();
    //public int points = 0;
    //public string selectedFaction = "";


    private List<string> sceneHistory = new List<string>();
    private string currentSceneName = null;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistente entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (sceneList == null)
        {
            Debug.LogError("SceneListSO is not assigned in the Inspector! Scene management will not function. GameManager()");
            enabled = false; // Disable the script if SceneListSO is missing
            return;
        }

        if (currentSceneName == null)
        {
            currentSceneName = SceneManager.GetActiveScene().name;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) // Press 'D' for Debug
        {
            Debug.Log("Current Scene: " + currentSceneName);
            Debug.Log("Scene History Count: " + sceneHistory.Count);
            Debug.Log("Scene History: " + string.Join(", ", sceneHistory.ToArray()));

            if (sceneList != null)
            {
                Debug.Log("Scene List Count: " + sceneList.sceneNames.Count);
                Debug.Log("Scene List: " + string.Join(", ", sceneList.sceneNames.ToArray()));
            }
            else
            {
                Debug.LogError("SceneListSO is NULL!");
            }
        }
    }


    public void ChangeScene(string newSceneName)
    {
        if (!IsValidScene(newSceneName))
        {
            Debug.LogError("Invalid scene name: " + newSceneName);
            return;
        }

        // Push the current scene to the history stack
        if (currentSceneName != null)
        {
            if (sceneHistory.Count >= maxScenesInStack)
            {
                Log("Scene history is full. Removing the oldest scene.");
                sceneHistory.RemoveAt(0); // FIFO
            }
            sceneHistory.Add(currentSceneName);
        }

        // Change to the new scene
        currentSceneName = newSceneName;
        LoadScene(currentSceneName);

        Log("Changed to scene: " + currentSceneName + ". Scene History: " + string.Join(", ", sceneHistory));
    }


    // Public method to go back to the previous scene
    public void GoBack()
    {
        if (sceneHistory.Count > 0)
        {
            // Get the previous scene from the history stack
            string previousSceneName = sceneHistory[sceneHistory.Count - 1];
            sceneHistory.RemoveAt(sceneHistory.Count - 1);

            if (!IsValidScene(previousSceneName))
            {
                Debug.LogError("Invalid scene name in history: " + previousSceneName + ". Ignoring.");
                return;
            }

            // Change to the previous scene
            currentSceneName = previousSceneName;
            LoadScene(currentSceneName);

            Log("Went back to scene: " + currentSceneName + ". Scene History: " + string.Join(", ", sceneHistory));
        }
        else
        {
            Log("No previous scenes in history.");
            // Optional: Go back to the main menu if history is empty
            // ChangeScene(sceneList.sceneNames[0]);
        }
    }


    // Helper method to load a scene
    private void LoadScene(string sceneName)
    {
        Log("Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }


    // Helper method to validate if a scene name is in the list
    private bool IsValidScene(string sceneName)
    {
        return sceneList.sceneNames.Contains(sceneName);
    }

    // Helper method to simplify Debug.Log calls based on enableDebugLogs
    private void Log(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log("[GameManager] " + message);
        }
    }

    // Public methods to expose all of the scene names (for buttons etc.)
    public List<string> GetSceneNames()
    {
        return sceneList.sceneNames;
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
