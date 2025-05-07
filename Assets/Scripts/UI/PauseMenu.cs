using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public Button resumeButton;
    public Button quitButton;
    public Button volumeUpButton;
    public Button volumeDownButton;

    public float volumeStep = 0.1f; // Cuánto sube/baja cada vez
    private bool isPaused = false;
    public Image volumeBar; // Arrástralo desde Unity


void Start()
{
    Time.timeScale = 1f; // Despausa forzadamente al iniciar

    if (pauseMenuUI != null)
        pauseMenuUI.SetActive(false);

    // Asignar eventos solo si los botones existen
    if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
    if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
    if (volumeUpButton != null) volumeUpButton.onClick.AddListener(IncreaseVolume);
    if (volumeDownButton != null) volumeDownButton.onClick.AddListener(DecreaseVolume);

    float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
    AudioListener.volume = savedVolume;

    UpdateVolumeBar(); // Actualiza la barra de volumen al iniciar
    Debug.Log("Start completado. Volume: " + AudioListener.volume);

}

void Update()
{
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        Debug.Log("ESCAPE DETECTADO");

        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }
}

void UpdateVolumeBar()
{
    if (volumeBar != null)
    {
        volumeBar.fillAmount = AudioListener.volume;
    }
}


public void PauseGame()
{
    pauseMenuUI.SetActive(true);
    Time.timeScale = 0f;         // Congela el tiempo
    isPaused = true;
}


public void ResumeGame()
{
    pauseMenuUI.SetActive(false); // Oculta el menú
    Time.timeScale = 1f;          // ¡Muy importante! Reanuda el tiempo
    isPaused = false;
}


    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

void IncreaseVolume()
{
    AudioListener.volume = Mathf.Clamp(AudioListener.volume + volumeStep, 0f, 1f);
    PlayerPrefs.SetFloat("MasterVolume", AudioListener.volume);
    UpdateVolumeBar();
}

void DecreaseVolume()
{
    AudioListener.volume = Mathf.Clamp(AudioListener.volume - volumeStep, 0f, 1f);
    PlayerPrefs.SetFloat("MasterVolume", AudioListener.volume);
    UpdateVolumeBar();
}


    public void TogglePauseMenu()
{
    if (isPaused)
        ResumeGame();
    else
        PauseGame();
}

}
