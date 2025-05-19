using UnityEngine;

public class CookingStation : MonoBehaviour
{
    public string stationType; // "Pan", "Pot", "Oven", etc.

    public void ShowRecipes()
    {
        RecipeManager recipeManager = FindFirstObjectByType<RecipeManager>();
        if (recipeManager == null)
        {
            Debug.LogWarning("No se encontró el RecipeManager.");
            return;
        }

        Recipe[] recetas = recipeManager.GetRecipesForUtensil(stationType);
        if (recetas == null || recetas.Length == 0)
        {
            Debug.Log("No hay recetas disponibles para esta estación.");
            return;
        }

        Debug.Log($"Recetas para {stationType}:");
        foreach (var receta in recetas)
        {
            Debug.Log("- " + receta.name);
        }

    }
}
