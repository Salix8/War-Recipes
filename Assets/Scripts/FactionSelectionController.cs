using UnityEngine;
using UnityEngine.SceneManagement;

public class FactionSelectionController : MonoBehaviour
{
    public void SelectFaction1Verde()
    {
        //GameManager.Instance.selectedFaction = "Faction1";
        BackButton.SetPreviousScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Mapa1Verde");
    }

    public void SelectFaction2Azul()
    {
        //GameManager.Instance.selectedFaction = "Faction2";
        BackButton.SetPreviousScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Mapa2Azul");
    }

    public void SelectFaction3Rojo()
    {
        //GameManager.Instance.selectedFaction = "Faction3";
        BackButton.SetPreviousScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Mapa3Rojo");
    }

}
