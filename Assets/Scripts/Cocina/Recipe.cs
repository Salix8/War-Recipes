using UnityEngine;

public enum StationType
{
    Pan,
    Pot,
    Oven,
    Fridge,
    Picar
}

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;
    public StationType stationType;
    public GameObject resultPrefab;
    public float cookingTime = 3f;
    public Ingredient[] ingredients;
}
