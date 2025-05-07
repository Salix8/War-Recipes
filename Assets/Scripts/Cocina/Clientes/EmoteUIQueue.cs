using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteUIQueue : MonoBehaviour
{
    public static EmoteUIQueue Instance;

    [Header("UI Settings")]
    public RectTransform emoteContainer; // Panel donde se colocan los emotes
    public GameObject emoteUIPrefab;    // Prefab de UI (con Image)
    public float spacing = 200f;         // Espacio horizontal entre emotes
    public float displayTime = 6f;      // Cuánto dura cada emote en pantalla
    public float fadeDuration = 2f;

    private List<GameObject> emoteQueue = new List<GameObject>();
    private bool isProcessing = false;

    void Awake()
    {
        Instance = this;
    }

    public void AddEmote(Sprite sprite)
    {
        if (sprite == null || sprite.name.ToLower().Contains("default")) return;

        GameObject emote = Instantiate(emoteUIPrefab, emoteContainer);
        emote.GetComponent<Image>().sprite = sprite;
        emote.GetComponent<CanvasGroup>().alpha = 1f;
        emoteQueue.Add(emote);
        UpdateQueuePositions();

        if (!isProcessing)
            StartCoroutine(ProcessQueue());
    }

    private void UpdateQueuePositions()
    {
        for (int i = 0; i < emoteQueue.Count; i++)
        {
            RectTransform rect = emoteQueue[i].GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(i * spacing, 0f);
        }
    }

    private IEnumerator ProcessQueue()
    {
        isProcessing = true;

        while (emoteQueue.Count > 0)
        {
            GameObject current = emoteQueue[0];
            yield return new WaitForSeconds(displayTime);

            // Fade out
            CanvasGroup group = current.GetComponent<CanvasGroup>();
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                group.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                yield return null;
            }

            emoteQueue.RemoveAt(0);
            Destroy(current);
            UpdateQueuePositions();
        }

        isProcessing = false;
    }
}
