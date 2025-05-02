using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EscurridorBehaviour : MonoBehaviour
{
    public GameObject escurridorVacio;
    public GameObject escurridorLleno;
    public GameObject canvasEscurridor;
    public Button btnCogerPlato;
    public GameObject player;
    private Inventory playerInv;
    public TMP_Text cantidad;

    void Start()
    {
        player = GameObject.Find("RangerB");
        playerInv = player.GetComponent<Inventory>();

        SinPlatos(false);
        canvasEscurridor.SetActive(false);
        UpdateEscurridor();
        
        btnCogerPlato.onClick.AddListener(CogerPlatoLimpio);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasEscurridor.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasEscurridor.SetActive(false);
        }
    }

    public void CogerPlatoLimpio() {
        if (!playerInv.sePuedeCogerPlato) {
            if (KitchenManager.Instance.CogerPlato()) { 
                playerInv.sePuedeCogerPlato = true;
                UpdateEscurridor();
            }
        }
    }

    public void UpdateEscurridor() {
        bool boleano = !playerInv.sePuedeCogerPlato;
        btnCogerPlato.gameObject.SetActive(boleano);

        cantidad.text = KitchenManager.Instance.CantPlatos().ToString() + "/" + KitchenManager.maxPlatos.ToString();

        SinPlatos(KitchenManager.Instance.CantPlatos() <= 0);
    }

    public void SinPlatos(bool boolean) {
        escurridorVacio.SetActive(boolean);
        escurridorLleno.SetActive(!boolean);
    }
}
