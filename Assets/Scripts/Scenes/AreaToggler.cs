using UnityEngine;

public class AreaToggler : MonoBehaviour
{
    [SerializeField] private GameObject[] toggleObjects; // Prefab del área a alternar
    public void ToggleArea(bool value)
    {
        foreach (GameObject obj in toggleObjects)
        {
            obj.SetActive(value);
        }
    }
    [ContextMenu("Disable Area")]
    void DisableArea()
    {
        ToggleArea(false);
    }
    [ContextMenu("Enable Area")]
    void EnableArea()
    {
        ToggleArea(true);
    }
}
