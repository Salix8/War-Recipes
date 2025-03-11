using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel; // Asignar en el Inspector el Panel de la UI
    public Button closeButton; // Asignar el botón de cerrar
    private bool isPopupActive = false;

    void Start()
    {
        // Asegurar que el panel está oculto al inicio
        popupPanel.SetActive(false);

        // Suscribir el botón a la función de cerrar
        closeButton.onClick.AddListener(ClosePopup);

        // Mostrar tutorial al inicio y pausar el juego
        ShowPopup(true);
    }

    void Update()
    {
        // Permitir cerrar el popup con la tecla "Escape"
        if (isPopupActive && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ClosePopup();
        }
    }

    // Mostrar el popup y detener el tiempo si es tutorial
    public void ShowPopup(bool pauseGame)
    {
        popupPanel.SetActive(true);
        isPopupActive = true;

        if (pauseGame)
        {
            Time.timeScale = 0f; // Pausar el juego
        }
    }

    // Cerrar el popup y reanudar el juego si estaba pausado
    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        isPopupActive = false;
        Time.timeScale = 1f; // Reanudar el juego
    }
}



/* Caso de uso
public class PopupTrigger : MonoBehaviour
{
    public PopupManager popupManager; // Arrastra aquí el GameObject con el script PopupManager

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tenga el tag "Player"
        {
            popupManager.ShowPopup(false); // Mostrar popup sin pausar el juego
        }
    }
}
*/