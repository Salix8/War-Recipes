using UnityEngine;
using UnityEngine.UI;

public class FregaderoBehaviour : MonoBehaviour
{
    public GameObject fregaderoVacio;
    public GameObject fregaderoLleno;
    public GameObject canvasFregadero;
    public Button btnDejarPlatos;
    public Button btnLavarPlatos;
    
    private int platosSucios = 0;
    private MeshFilter meshFilter;
    private bool jugadorEnZona = false;
    private Inventory inventarioJugador;

    void Start()
    {
        fregaderoVacio.SetActive(true);
        fregaderoLleno.SetActive(false);
        canvasFregadero.SetActive(false);
        btnDejarPlatos.onClick.AddListener(AgregarPlatos);
        btnLavarPlatos.onClick.AddListener(Vaciar);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = true;
            inventarioJugador = other.GetComponent<Inventory>();
            canvasFregadero.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = false;
            inventarioJugador = null;
            canvasFregadero.SetActive(false);
        }
    }

    public void AgregarPlatos()
    {
        if (inventarioJugador.itemEnMano == "platos sucios" && inventarioJugador.cantidadItem > 0)
        {
            platosSucios += inventarioJugador.cantidadItem;

            if (platosSucios > 5)
            {
                CambiarEstadoFregadero(true);
            }

            inventarioJugador.VaciarInventario();
        }
    }

    public void Vaciar() 
    {
        platosSucios = 0;
        CambiarEstadoFregadero(false);
    }

    private void CambiarEstadoFregadero(bool lleno)
    {
        fregaderoVacio.SetActive(!lleno);
        fregaderoLleno.SetActive(lleno);
    }
}
