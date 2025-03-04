using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importa el namespace de TextMeshPro
using System.Collections.Generic;

public class ButtonGenerator : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject buttonPrefab;
    public Transform buttonParent;

    void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager no ha sido asignado. Revisa el inspector. ButtonGenerator()");
            return;
        }

        if (buttonPrefab == null)
        {
            Debug.LogError("Button Prefab no ha sido asignado. Revisa el inspector. ButtonGenerator()");
            return;
        }

        if (buttonParent == null)
        {
            Debug.LogError("Button Parent no ha sido asignado. Revisa el inspector. ButtonGenerator()");
            return;
        }
        GenerateButtons();
    }
    void GenerateButtons()
    {
        List<string> sceneNames = gameManager.GetSceneNames();

        // Iterate through the scene names and create buttons
        foreach (string sceneName in sceneNames)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent);

            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = sceneName;
            }

            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                // Add a listener to the button to call the ChangeScene method
                string sceneNameToLoad = sceneName;
                buttonComponent.onClick.AddListener(() => gameManager.ChangeScene(sceneNameToLoad));
            }
            else
            {
                Debug.LogError("Button component not found on button prefab.");
            }
        }
    }
}