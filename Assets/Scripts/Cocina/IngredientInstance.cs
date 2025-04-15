using UnityEngine;

public class IngredientInstance : MonoBehaviour
{
    [SerializeField] Ingredient ingredient;

    public Ingredient GetIngredient()
    {
        return ingredient;
    }

}
