using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public Recipe[] panRecipes;
    public Recipe[] potRecipes;
    public Recipe[] ovenRecipes;

    public Recipe[] GetRecipesForUtensil(string utensilName)
    {
        switch (utensilName)
        {
            case "Pan":
                return panRecipes;
            case "Pot":
                return potRecipes;
            case "Oven":
                return ovenRecipes;
            default:
                return null;
        }
    }
}
