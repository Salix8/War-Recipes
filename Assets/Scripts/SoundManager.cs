using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Play(string soundName)
    {
        Debug.Log("Reproduciendo sonido: " + soundName);
        // Aquí deberías reproducir un AudioClip según el nombre
    }
}

