using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para acceder a las funciones de botón

public class BackButton : MonoBehaviour
{
    private static string previousSceneName = null; // Variable estática para guardar el nombre de la escena anterior

    [Tooltip("Nombre de la escena a la que volver si no hay historial.")]
    public string defaultSceneName = "MainMenu"; //  Una escena por defecto si no se ha visitado una escena anterior.

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError($"En la escena {SceneManager.GetActiveScene().name} El GameObject {this.name} no tiene un componente Button. BackButton()");
            enabled = false; // Desactiva el script si no hay Button.
            return;
        }

        // Agrega un listener al evento onClick del botón.
        button.onClick.AddListener(OnBackButton);
    }


    public void OnBackButton()
    {
        Debug.Log(previousSceneName);
        if (previousSceneName != null)
        {
            SceneManager.LoadScene(previousSceneName);
            previousSceneName = null;
        }
        else
        {
            // Si no hay escena anterior, carga la escena por defecto.
            SceneManager.LoadScene(defaultSceneName);
            Debug.LogWarning("No hay escena anterior en el historial. Cargando la escena por defecto: " + defaultSceneName);
        }
    }

    // Método estático para establecer la escena anterior.  Se llama ANTES de cambiar de escena.
    public static void SetPreviousScene(string sceneName)
    {
        previousSceneName = sceneName;
    }
}