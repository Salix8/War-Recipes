using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CookingCrateManager : MonoBehaviour
{
    public static CookingCrateManager Instance;

    [System.Serializable]
    public class CrateVisual
    {
        public string ingredientName;
        public int startingAmount = 0;

        public GameObject crateEmpty;   // Sprite vacío (dentro del crate)
        public GameObject crateFull;    // Sprite lleno (dentro del crate)
        public TextMeshPro amountText;  // Texto visible encima del crate
    }

    public CrateVisual[] crateVisuals;

    private Dictionary<string, CrateVisual> visualDict = new Dictionary<string, CrateVisual>();
    private Dictionary<string, int> ingredientAmounts = new Dictionary<string, int>();

    void Awake()
    {
        Instance = this;

        foreach (var visual in crateVisuals)
        {
            // Registrar cada ingrediente
            visualDict[visual.ingredientName] = visual;

            // Guardar cantidad inicial
            ingredientAmounts[visual.ingredientName] = visual.startingAmount;

            // Asegurar que el estado visual inicial es correcto
            UpdateCrates(visual.ingredientName, visual.startingAmount);
        }
    }

    public void UpdateCrates(string ingredient, int amount)
    {
        if (!visualDict.ContainsKey(ingredient)) return;

        ingredientAmounts[ingredient] = amount;
        var visual = visualDict[ingredient];
        bool hasItem = amount > 0;

        // Mostrar u ocultar sprites
        if (visual.crateEmpty != null)
            visual.crateEmpty.SetActive(!hasItem);

        if (visual.crateFull != null)
            visual.crateFull.SetActive(hasItem);

        // Actualizar el texto de cantidad
        if (visual.amountText != null)
        {
            visual.amountText.text = amount.ToString();
            visual.amountText.gameObject.SetActive(true);
        }
    }

    public void ModifyIngredient(string ingredient, int delta)
    {
        if (!ingredientAmounts.ContainsKey(ingredient)) return;

        int newAmount = Mathf.Max(0, ingredientAmounts[ingredient] + delta);
        UpdateCrates(ingredient, newAmount);
    }

    void LateUpdate()
    {
        // Siempre hacer que el texto mire hacia la cámara
        foreach (var visual in visualDict.Values)
        {
            if (visual.amountText != null)
                visual.amountText.transform.forward = Camera.main.transform.forward;
        }
    }
}
