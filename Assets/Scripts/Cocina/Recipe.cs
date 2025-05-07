using UnityEngine;
using System.Collections.Generic;  

[System.Serializable]
public class Recipe
{
    public string name;
    public Sprite icon;
    public List<string> ingredients;
    public float cookingTime;
}
