using UnityEngine;

public class PrefabDatabase : MonoBehaviour
{
    public static PrefabDatabase Instance;

    public GameObject heladoPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public GameObject GetHeladoPrefab()
    {
        return heladoPrefab;
    }
}
