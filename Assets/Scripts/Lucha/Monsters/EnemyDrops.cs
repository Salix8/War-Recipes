using UnityEngine;

[System.Serializable]
public class Drop
{
    public string itemName;
    public int amount = 1;
}

public class EnemyDrops : MonoBehaviour
{
    public Drop[] drops;

    public void DropItems()
    {
        foreach (Drop drop in drops)
        {
            if (!string.IsNullOrEmpty(drop.itemName) && drop.amount > 0)
            {
                // Aumenta el conteo de ingredientes en el CookingCrateManager
                CookingCrateManager.Instance.ModifyIngredient(drop.itemName, drop.amount);

                // Actualiza el modelo visual en la caja flotante
                if (SmoothFollowBox.Instance != null)
                {
                    SmoothFollowBox.Instance.ShowCollectedItem(drop.itemName);
                }
            }
        }
    }
}
