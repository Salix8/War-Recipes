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
        public int startingAmount = 0;               // Puedes definir cuántos hay al inicio
        public GameObject crateEmpty;                // Modelo vacío (ya en la escena)
        public GameObject crateFull;                 // Modelo lleno (ya en la escena)
        public TextMeshPro amountText;               // Texto encima de la caja
    }

    public CrateVisual[] crateVisuals;

    private Dictionary<string, CrateVisual> visualDict = new Dictionary<string, CrateVisual>();
    private Dictionary<string, int> ingredientAmounts = new Dictionary<string, int>();

    void Awake()
    {
        Instance = this;

        foreach (var visual in crateVisuals)
        {
            visualDict[visual.ingredientName] = visual;
            ingredientAmounts[visual.ingredientName] = visual.startingAmount;
            UpdateCrates(visual.ingredientName, visual.startingAmount);
        }
    }

    public void UpdateCrates(string ingredient, int amount)
    {
        if (!visualDict.ContainsKey(ingredient)) return;

        ingredientAmounts[ingredient] = amount;
        var visual = visualDict[ingredient];
        bool hasItem = amount > 0;

        if (visual.crateEmpty != null)
            visual.crateEmpty.SetActive(!hasItem);

        if (visual.crateFull != null)
            visual.crateFull.SetActive(hasItem);

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
        foreach (var visual in visualDict.Values)
        {
            if (visual.amountText != null)
                visual.amountText.transform.forward = Camera.main.transform.forward;
        }
    }
}
