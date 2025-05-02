using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public int platosSucios = 0;
    public Recetario recetario;
    public Dictionary<Ingrediente, int> inventarioIngredientes = new Dictionary<Ingrediente, int>();
    public Receta platoEnMano;
    public bool sePuedeCogerPlato = false;
    
    void Start()
    {
        CrearInventario();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CrearInventario() {
        recetario = GameObject.Find("Recetario").GetComponent<Recetario>();
        foreach (Ingrediente ing in recetario.ingredientes) {
            inventarioIngredientes.Add(ing, 2);
        }
    }

    public void VaciarInventarioPlatosSucios () {
        platosSucios = 0;
    }

    //SE QUITAN LOS INGREDIENTES CUANDO SE PREPARAN RECETAS
    public void QuitarIngrediente(Ingrediente ingrediente, int cantidad) {
        inventarioIngredientes[ingrediente] = inventarioIngredientes[ingrediente] - cantidad;
    }

    //COGER PLATO CON COMIDA
    public void CogerPlato(Receta nuevoPlato) {
        platoEnMano = nuevoPlato;
        sePuedeCogerPlato = false;
    }

    public bool SePuedeCogerPlato() {
        return sePuedeCogerPlato;
    }
}
