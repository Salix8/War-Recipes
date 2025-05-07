using System.Collections.Generic;
using UnityEngine;

public class CharacterManagerCocina : MonoBehaviour
{
    public static CharacterManagerCocina Instance;

    private Dictionary<string, int> ingredients = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool HasIngredients(List<string> recipeIngredients)
    {
        Dictionary<string, int> required = new Dictionary<string, int>();

        // Contar cuántos de cada ingrediente se necesitan
        foreach (string ingredient in recipeIngredients)
        {
            if (required.ContainsKey(ingredient))
                required[ingredient]++;
            else
                required[ingredient] = 1;
        }

        // Verificar si los tenemos todos
        foreach (var kvp in required)
        {
            if (!ingredients.ContainsKey(kvp.Key) || ingredients[kvp.Key] < kvp.Value)
                return false;
        }

        return true;
    }

    public void UseIngredients(List<string> recipeIngredients)
    {
        Dictionary<string, int> toUse = new Dictionary<string, int>();

        foreach (string ingredient in recipeIngredients)
        {
            if (toUse.ContainsKey(ingredient))
                toUse[ingredient]++;
            else
                toUse[ingredient] = 1;
        }

        foreach (var kvp in toUse)
        {
            if (ingredients.ContainsKey(kvp.Key))
            {
                ingredients[kvp.Key] -= kvp.Value;
                if (ingredients[kvp.Key] <= 0)
                    ingredients.Remove(kvp.Key);

                CookingCrateManager.Instance?.UpdateCrates(kvp.Key, ingredients.ContainsKey(kvp.Key) ? ingredients[kvp.Key] : 0);
            }
        }
    }

    public void AddIngredient(string ingredientName, int amount)
    {
        if (ingredients.ContainsKey(ingredientName))
            ingredients[ingredientName] += amount;
        else
            ingredients[ingredientName] = amount;

        CookingCrateManager.Instance?.UpdateCrates(ingredientName, ingredients[ingredientName]);
    }

    public void MoveToStation(Transform stationTransform)
    {
        // Lógica para mover al personaje si quieres
        Debug.Log("Moviendo personaje a estación: " + stationTransform.name);
    }
}
