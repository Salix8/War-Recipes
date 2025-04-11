using UnityEngine;

public class TransformIngredient : MonoBehaviour
{
    [SerializeField] private Ingredient transformedIngredient;
    [SerializeField] private TransformType transformType;

    public enum TransformType
    {
        Cut,
        Cook,
        Fry
    }
}
