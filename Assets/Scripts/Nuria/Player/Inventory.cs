using UnityEngine;

public class Inventory : MonoBehaviour
{
    public string itemEnMano = "platos sucios";
    public int cantidadItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VaciarInventario () {
        itemEnMano = "";
        cantidadItem = 0;
    }
}
