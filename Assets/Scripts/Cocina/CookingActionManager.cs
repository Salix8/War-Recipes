using UnityEngine;
using System.Collections;
using System;

public enum CookingActionType
{
    None,
    Waiting,
    Clicking
}

public class CookingActionManager : MonoBehaviour
{
    public static CookingActionManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartCookingAction(Recipe recipe, CookingActionType actionType, Action onComplete)
    {
        switch (actionType)
        {
            case CookingActionType.Waiting:
                StartCoroutine(DoWaiting(recipe.cookingTime, onComplete));
                break;
            case CookingActionType.Clicking:
                StartCoroutine(DoClicking(5, onComplete)); // 5 clicks requeridos por ejemplo
                break;
            default:
                Debug.LogWarning("Tipo de acción desconocida.");
                break;
        }
    }

    private IEnumerator DoWaiting(float duration, Action onComplete)
    {
        Debug.Log("Esperando " + duration + " segundos para cocinar...");
        yield return new WaitForSeconds(duration);
        onComplete?.Invoke();
    }

    private IEnumerator DoClicking(int requiredClicks, Action onComplete)
    {
        Debug.Log("Pulsa para cocinar: necesitas " + requiredClicks + " clics.");
        int clicks = 0;

        while (clicks < requiredClicks)
        {
            if (Input.GetMouseButtonDown(0))
            {
                clicks++;
                Debug.Log("Click " + clicks + "/" + requiredClicks);
            }
            yield return null;
        }

        onComplete?.Invoke();
    }
}
