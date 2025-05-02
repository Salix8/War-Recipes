using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Receta
{
    public int id;
    public string nombre;
    public string ptoAccion;
    public Sprite btnIcono;
    public int cantidadPlatos;
    public int precioPlato;
    public int clicsPreparacion;
    public Dictionary<Ingrediente, int> ingredientes;

    //Constructor
    public Receta(int _id, string _nombre, string _ptoAccion, Sprite _btnIcono, int _cantidad, int _precio, int _clics, Dictionary<Ingrediente, int> _ingredientes) {
        id = _id;
        nombre = _nombre;
        ptoAccion = _ptoAccion;
        btnIcono = _btnIcono;
        cantidadPlatos = _cantidad;
        precioPlato = _precio;
        clicsPreparacion = _clics;
        ingredientes = _ingredientes;
    }

    // public int getId(){
    //     return id;
    // }
}

public class Ingrediente
{
    public int id;
    public string nombre;
    public Sprite icono;

    public Ingrediente(int _id, string _nombre, Sprite _icono) {
        id = _id;
        nombre = _nombre;
        icono = _icono;
    }
}