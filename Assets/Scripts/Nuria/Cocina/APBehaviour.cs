using UnityEngine;
using System.Collections.Generic;

public class APBehaviour : MonoBehaviour, IPuntoDeAccion
{
    public string nombre;
    public GameObject modeloInactivo;
    public GameObject modeloActivo;
    public GameObject canvas;
    private CanvasBehaviour scriptCanvas;
    public Recetario recetario;
    public bool isPlayerInZone = false;

    void Start()
    {
        scriptCanvas = canvas.GetComponent<CanvasBehaviour>();
        recetario = GameObject.Find("Recetario").GetComponent<Recetario>();
        modeloInactivo.SetActive(true);
        modeloActivo.SetActive(false);
        canvas.SetActive(false);
    }

    public string GetName() => nombre;
    public bool CanBurn() => false;
    public bool IsPlayerInZone() => isPlayerInZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(true);
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            if (!scriptCanvas.recogerPlato) {
                scriptCanvas.ResetCanvas();
                canvas.SetActive(false);
            }
            isPlayerInZone = false;
        }
    }

    public void CambiarEstado(bool activo)
    {
        modeloInactivo.SetActive(!activo);
        modeloActivo.SetActive(activo);
    }
}
