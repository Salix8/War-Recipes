using UnityEngine;

public class FregaderoBehaviour : MonoBehaviour
{
    public GameObject fregaderoVacio;
    public GameObject fregaderoLleno;
    public int platosSucios = 0;

    private MeshFilter meshFilter;
    private bool jugadorEnZona = false;
    private Inventory inventarioJugador;

    void Start()
    {
        fregaderoVacio.SetActive(true);
        fregaderoLleno.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = false;
            
        }
    }

    void Update()
    {
        if (jugadorEnZona)
        {
            Debug.Log("El jugador ha entrado en el área");
        } else Debug.Log("El jugador ha salido del área");
    }

    public void AgregarPlato()
    {
        platosSucios++;

        if (platosSucios > 5)
        {
            CambiarEstadoFregadero(true);
        }
    }

    public void QuitarPlato()
    {
        if (platosSucios > 0)
        {
            platosSucios--;

            if (platosSucios <= 5)
            {
                CambiarEstadoFregadero(false);
            }
        }
    }

    private void CambiarEstadoFregadero(bool lleno)
    {
        fregaderoVacio.SetActive(!lleno);
        fregaderoLleno.SetActive(lleno);
    }
}
