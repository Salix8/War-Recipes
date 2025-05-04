using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerEmoteController : MonoBehaviour
{
    [Header("Emote Settings")]
    public GameObject emoteRoot; // El objeto contenedor del emote (debería estar en la cabeza)
    public SpriteRenderer emoteRenderer;

    [Header("Emote Sprites")]
    public Sprite emoteDefault;
    public Sprite emoteBien;
    public Sprite emoteFeliz;
    public Sprite emoteEncantado;
    public Sprite emoteEnfadado1;
    public Sprite emoteEnfadado2;
    public Sprite emoteEnfadado3;

    [Header("Timers (para enfado)")]
    public float timeToEnfadado1 = 10f;
    public float timeToEnfadado2 = 20f;
    public float timeToEnfadado3 = 30f;

    private float waitTime = 0f;
    private bool isWaiting = false;

    void Start()
    {
        if (emoteRoot != null)
            emoteRoot.SetActive(false);
    }

    void Update()
    {
        // Hacer que el emote mire a la cámara
        if (emoteRoot != null && Camera.main != null)
        {
            emoteRoot.transform.rotation = Quaternion.LookRotation(emoteRoot.transform.position - Camera.main.transform.position);
        }

        // Control de tiempo de espera para enfados
        if (isWaiting)
        {
            waitTime += Time.deltaTime;

            if (waitTime >= timeToEnfadado3)
            {
                ShowEmote(emoteEnfadado3);
                isWaiting = false;
            }
            else if (waitTime >= timeToEnfadado2)
            {
                ShowEmote(emoteEnfadado2);
            }
            else if (waitTime >= timeToEnfadado1)
            {
                ShowEmote(emoteEnfadado1);
            }
        }
    }

    public void StartWaiting()
    {
        isWaiting = true;
        waitTime = 0f;
    }

    public void StopWaiting()
    {
        isWaiting = false;
        waitTime = 0f;
        HideEmote();
    }

    public void ShowDefaultEmote()
    {
        ShowEmote(emoteDefault);
    }

    public void ShowBienEmote()
    {
        ShowEmote(emoteBien);
    }

    public void ShowFelizEmote()
    {
        ShowEmote(emoteFeliz);
    }

    public void ShowEncantadoEmote()
    {
        ShowEmote(emoteEncantado);
    }

    private void ShowEmote(Sprite sprite)
    {
        if (emoteRenderer != null && sprite != null)
        {
            emoteRenderer.sprite = sprite;
            emoteRoot.SetActive(true);
        }
    }

    public void HideEmote()
    {
        if (emoteRoot != null)
        {
            emoteRoot.SetActive(false);
        }
    }
}
