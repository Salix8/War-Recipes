using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void GoBack()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoBack();
        }
        else
        {
            Debug.LogError("GameManager.Instance is null or no scene selected! Aseg�rate de que el GameManager se haya inicializado correctamente. BackButton()");
        }
    }
}
