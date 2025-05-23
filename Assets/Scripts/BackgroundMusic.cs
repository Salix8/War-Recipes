using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;

    [System.Serializable]
    public class EscenarioMusic
    {
        public string nombreEscenario;
        public AudioClip musicClip;
    }

    [Header("Configuración")]
    public List<EscenarioMusic> musicPorEscenario;

    private AudioSource audioSource;
    private string escenarioActual = "";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
    }

    public void CambiarEscenario(string nombre)
    {
        if (nombre == escenarioActual) return;

        EscenarioMusic musica = musicPorEscenario.Find(m => m.nombreEscenario == nombre);
        if (musica != null && musica.musicClip != null)
        {
            audioSource.clip = musica.musicClip;
            audioSource.Play();
            escenarioActual = nombre;
        }
        else
        {
            Debug.LogWarning("[BackgroundMusic] No se encontró música para el escenario: " + nombre);
            audioSource.Stop();
            escenarioActual = "";
        }
    }

    public void SetVolume(float volumen)
    {
        audioSource.volume = Mathf.Clamp01(volumen);
    }

    public void PararMusica()
    {
        audioSource.Stop();
        escenarioActual = "";
    }
}