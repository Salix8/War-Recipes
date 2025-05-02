using UnityEngine;

public class KitchenManager : MonoBehaviour
{
    public static KitchenManager Instance { get; private set; }
    public const int maxPlatos = 15;
    private int stackPlatos;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    void Start()
    {
        stackPlatos = maxPlatos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CogerPlato() {
        if (stackPlatos <= 0) return false;
        stackPlatos--;
        return true;
    }

    public void ReponerPlatos() {
        stackPlatos = maxPlatos;
    }

    public int CantPlatos() { return stackPlatos;}
}
