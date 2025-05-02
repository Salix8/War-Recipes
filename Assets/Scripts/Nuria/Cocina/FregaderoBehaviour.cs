using UnityEngine;
using UnityEngine.UI;

public class FregaderoBehaviour : MonoBehaviour
{
    public GameObject fregaderoVacio;
    public GameObject fregaderoLleno;
    public GameObject canvasFregadero;
    public Button btnDejarPlatos;
    public Button btnLavarPlatos;
    public Button btnSpam;
    public Slider spamSlider;
    
    private int clickCounter;
    private int platosSucios = 0;
    private Inventory inventarioJugador;

    void Start()
    {
        CambiarEstado(false);
        canvasFregadero.SetActive(false);
        btnDejarPlatos.onClick.AddListener(AgregarPlatos);
        btnLavarPlatos.onClick.AddListener(Vaciar);
        btnSpam.onClick.AddListener(Limpiando);
        BtnsVisibility();
    }

    private void BtnsVisibility ()
    {
        if (platosSucios <= 0) btnLavarPlatos.gameObject.SetActive(false);
        else btnLavarPlatos.gameObject.SetActive(true);
        if (inventarioJugador != null) { 
            if (inventarioJugador.platosSucios <= 0) btnDejarPlatos.gameObject.SetActive(false);
            else btnDejarPlatos.gameObject.SetActive(true);
        }
        btnSpam.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventarioJugador = other.GetComponent<Inventory>();
            canvasFregadero.SetActive(true);

            BtnsVisibility();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventarioJugador = null;
            canvasFregadero.SetActive(false);
            clickCounter = 0;
            spamSlider.value = 0;
        }
    }

    public void AgregarPlatos()
    {
        if (inventarioJugador.platosSucios > 0)
        {
            platosSucios += inventarioJugador.platosSucios;

            if (platosSucios > 5)
            {
                CambiarEstado(true);
            }

            inventarioJugador.VaciarInventarioPlatosSucios();
            BtnsVisibility();
        }
    }

    public void Vaciar() 
    {
        btnDejarPlatos.gameObject.SetActive(false);
        btnLavarPlatos.gameObject.SetActive(false);
        btnSpam.gameObject.SetActive(true);
    }

    private void CambiarEstado(bool lleno)
    {
        fregaderoVacio.SetActive(!lleno);
        fregaderoLleno.SetActive(lleno);
    }

    private void Limpiando() {
        
        clickCounter++;
        if (clickCounter >= platosSucios) {
            platosSucios = 0;
            CambiarEstado(false);
            btnSpam.gameObject.SetActive(false);
            
        }
        if (spamSlider.value < 100) spamSlider.value += spamSlider.maxValue / platosSucios;
    }
}
