using System.Collections.Generic;
using UnityEngine;

public static class ItemManager
{
    private static Dictionary<Ingredient, int> ingredientes = new Dictionary<Ingredient, int>();

    [Header("Debug")]
    [Tooltip("Enable to log scene changes and stack status to the console.")]
    public static bool enableDebugLogs = false;

    private static void Log(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log("[ItemManager] " + message);
        }
    }

    public static void AñadirIngrediente(Ingredient ingrediente)
    {
        if (ingredientes.ContainsKey(ingrediente))
        {
            ingredientes[ingrediente]++;
            Log($"Cantidad de {ingrediente.name} incrementada a {ingredientes[ingrediente]}.");
        }
        else
        {
            ingredientes[ingrediente] = 1;
            Log($"Ingrediente añadido: {ingrediente.name} (x1)");
        }
    }

    public static void EliminarIngrediente(Ingredient ingrediente)
    {
        if (ingredientes.ContainsKey(ingrediente))
        {
            ingredientes[ingrediente]--;
            if (ingredientes[ingrediente] <= 0)
            {
                ingredientes.Remove(ingrediente);
                Log($"Ingrediente eliminado completamente: {ingrediente.name}");
            }
            else
            {
                Log($"Cantidad de {ingrediente.name} disminuida a {ingredientes[ingrediente]}.");
            }
        }
        else
        {
            Debug.LogWarning($"[ItemManager] Intento de eliminar un ingrediente no presente: {ingrediente.name}");
        }
    }

    public static void VaciarIngredientes()
    {
        ingredientes.Clear();
        Log("Lista de ingredientes vaciada.");
    }

    public static Dictionary<Ingredient, int> GetIngredientes()
    {
        return new Dictionary<Ingredient, int>(ingredientes);
    }

    public static bool TieneIngrediente(Ingredient ingrediente)
    {
        return ingredientes.ContainsKey(ingrediente);
    }

    public static int GetCantidad(Ingredient ingrediente)
    {
        return ingredientes.TryGetValue(ingrediente, out int cantidad) ? cantidad : 0;
    }
}
