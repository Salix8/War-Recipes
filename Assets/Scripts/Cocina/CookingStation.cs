using UnityEngine;

public class CookingStation : MonoBehaviour
{
    public string utensilName;
    private Recipe[] recipes;
    private RecipeManager recipeManager;

    void Start()
    {
        recipeManager = Object.FindFirstObjectByType<RecipeManager>();
        recipes = recipeManager.GetRecipesForUtensil(utensilName);
    }

    public void ShowRecipes()
    {
        Debug.Log("Mostrando recetas para: " + utensilName);
        foreach (var recipe in recipes)
        {
            Debug.Log("- " + recipe.name);
        }
        CharacterManagerCocina.Instance.MoveToStation(transform);
    }
}
