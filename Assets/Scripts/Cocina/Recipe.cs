using UnityEngine;

[System.Serializable]
public class Recipe
{
    public string name;
    public Sprite icon;   // Icono para el botón
    public string[] ingredients;
    public float cookingTime;  // Tiempo de preparación en segundos
}
