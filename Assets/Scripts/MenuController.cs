using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Se asigna al botón Play desde el Inspector
    public void OnPlayButton()
    {
        BackButton.SetPreviousScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("FactionSelection");
    }

    public void OnOptionsButton()
    {
        // Implementa las opciones según necesites
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
