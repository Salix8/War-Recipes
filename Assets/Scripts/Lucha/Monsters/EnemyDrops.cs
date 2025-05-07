using UnityEngine;

[System.Serializable]
public class Drop
{
    public string itemName;
    public int amount = 1; // Valor por defecto
}

public class EnemyDrops : MonoBehaviour
{
    public Drop[] drops;

    public void DropItems()
    {
        foreach (Drop drop in drops)
        {
            // Asegúrate de que el nombre no esté vacío y la cantidad sea positiva
            if (!string.IsNullOrEmpty(drop.itemName) && drop.amount > 0)
            {
                CharacterManagerCocina.Instance.AddIngredient(drop.itemName, drop.amount);
            }
        }
    }
}
