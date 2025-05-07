using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CookingStation : MonoBehaviour
{
    public string utensilName;
    private Recipe[] recipes;
    private RecipeManager recipeManager;

    private Dictionary<string, int> ingredients = new Dictionary<string, int>();

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

    public void CookRecipe(Recipe recipe)
    {
if (CharacterManagerCocina.Instance.HasIngredients(recipe.ingredients))
{
    CharacterManagerCocina.Instance.UseIngredients(recipe.ingredients);
    Debug.Log("Cocinando: " + recipe.name);
    StartCoroutine(CookingProcess(recipe));
}

        else
        {
            Debug.Log("Faltan ingredientes para: " + recipe.name);
        }
    }

    private IEnumerator CookingProcess(Recipe recipe)
    {
        Debug.Log("Cocinando " + recipe.name + " durante " + recipe.cookingTime + " segundos");
        yield return new WaitForSeconds(recipe.cookingTime);
        Debug.Log("¡Receta completada: " + recipe.name + "!");
    }

    public void UseIngredients(string[] usedIngredients)
    {
        foreach (string ingredient in usedIngredients)
        {
            if (ingredients.ContainsKey(ingredient))
            {
                ingredients[ingredient]--;
                if (ingredients[ingredient] <= 0)
                {
                    ingredients.Remove(ingredient);
                }

                CookingCrateManager.Instance?.UpdateCrates(ingredient, ingredients.ContainsKey(ingredient) ? ingredients[ingredient] : 0);
            }
        }
    }
}
