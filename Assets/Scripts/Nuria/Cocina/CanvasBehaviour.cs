using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class CanvasBehaviour : MonoBehaviour
{
    public Button btnReceta1;
    public Button btnReceta2;
    public Button btnReceta3;
    public Button btnReceta4;
    public Image ingrediente1;
    public Image ingrediente2;
    public Image ingrediente3;
    public Image ingrediente4;
    public TMP_Text cantidad1;
    public TMP_Text cantidad2;
    public TMP_Text cantidad3;
    public TMP_Text cantidad4;
    public Slider timerSlider;
    public Slider spamSlider;

    public Button btnSpam;
    public Button btnThrow;

    public GameObject ptoAccion;
    private IPuntoDeAccion script;
    private List<Receta> listaRecetas = new List<Receta>();
    private int recetaElegida = -1;
    private int recetaAnterior = -2;
    private bool suficientesIng;
    private Inventory playerInventory;
    public bool cocinando = false;
    public bool recogerPlato = false;
    public float speed = 15f;

    public Recetario recetario;

    void Start()
    {
        GameObject rangerB = GameObject.Find("RangerB");
        script = ptoAccion.GetComponent<IPuntoDeAccion>();
        playerInventory = rangerB.GetComponent<Inventory>();
        ObtenerListaRecetas();
        ActualizarBotones();
        ActualizarIngredientes();
    }

    void Update() {
        if (cocinando) {
            timerSlider.value += speed * Time.deltaTime;
            if (timerSlider.value >= timerSlider.maxValue) ResetCanvas();
        }
    }

    private void ObtenerListaRecetas() {
        recetario = GameObject.Find("Recetario").GetComponent<Recetario>();
        List<Receta> recetas = recetario.recetas;
        foreach (Receta receta in recetas)
        {
            if (receta.ptoAccion == script.GetName())
            {
                listaRecetas.Add(receta);
            }
        }
    }

    private void ElegirReceta (int recetaIndex) {
        recetaElegida = recetaIndex;
        ActualizarIngredientes();
    }

    private void ActualizarBotones () {
        btnReceta1.image.sprite = listaRecetas[0].btnIcono;
        btnReceta1.onClick.AddListener(() => ElegirReceta(0));
        btnReceta2.image.sprite = listaRecetas[1].btnIcono;
        btnReceta2.onClick.AddListener(() => ElegirReceta(1));
        if (listaRecetas.Count > 2) {
            btnReceta3.image.sprite = listaRecetas[2].btnIcono;
            btnReceta3.onClick.AddListener(() => ElegirReceta(2));
        } else {
            btnReceta3.gameObject.SetActive(false);
        }
        if (listaRecetas.Count > 3) {
            btnReceta4.image.sprite = listaRecetas[3].btnIcono;
            btnReceta4.onClick.AddListener(() => ElegirReceta(3));
        } else {
            btnReceta4.gameObject.SetActive(false);
        }

        btnSpam.gameObject.SetActive(false);
        btnSpam.onClick.AddListener(Spam);

        btnThrow.gameObject.SetActive(false);
        btnThrow.onClick.AddListener(Vaciar);
    }

    private void Vaciar() {
        if (script.IsPlayerInZone()) {
            QuitarIngredientes(recetaElegida);
            ResetCanvas();
        }
    }

    private void ActualizarIngredientes () {
        if (recetaElegida == -1) {
            ClearIngredientes();
        } else {
            if (recetaElegida == recetaAnterior) {
                if (suficientesIng) Cocinar();
            } else {
                ingrediente1.enabled = true;
                cantidad1.enabled = true;
                ingrediente2.enabled = true;
                cantidad2.enabled = true;

                suficientesIng = true;

                Dictionary<Ingrediente, int> DictIngredientes = listaRecetas[recetaElegida].ingredientes;
                int i = 1;

                foreach (KeyValuePair<Ingrediente, int> par in DictIngredientes) {
                    string nombreIngrediente = par.Key.nombre;
                    int playerCant = playerInventory.inventarioIngredientes
                        .Where(kvp => kvp.Key.nombre == nombreIngrediente)
                        .Select(kvp => kvp.Value)
                        .FirstOrDefault();

                    if (playerCant < par.Value) suficientesIng = false;

                    switch (i)
                    {
                        case 1:
                            ingrediente1.sprite = par.Key.icono;
                            IngredienteText(cantidad1, playerCant, par.Value);
                            break;
                        case 2:
                            ingrediente2.sprite = par.Key.icono;
                            IngredienteText(cantidad2, playerCant, par.Value);
                            break;
                        case 3:
                            ingrediente3.sprite = par.Key.icono;
                            IngredienteText(cantidad3, playerCant, par.Value);
                            break;
                        case 4:
                            ingrediente4.sprite = par.Key.icono;
                            IngredienteText(cantidad4, playerCant, par.Value);
                            break;
                    }
                    i++;
                }

                if (i>3) {
                    ingrediente3.enabled = true;
                    cantidad3.enabled = true;
                } else {
                    ingrediente3.enabled = false;
                    cantidad3.enabled = false;
                }

                if (i>4) {
                    ingrediente4.enabled = true;
                    cantidad4.enabled = true;
                } else {
                    ingrediente4.enabled = false;
                    cantidad4.enabled = false;
                }

                recetaAnterior = recetaElegida;
            }
        }
    }

    private void ClearIngredientes() {
        ingrediente1.enabled = false;
        cantidad1.enabled = false;
        ingrediente2.enabled = false;
        cantidad2.enabled = false;
        ingrediente3.enabled = false;
        cantidad3.enabled = false;
        ingrediente4.enabled = false;
        cantidad4.enabled = false;
    }

    private void IngredienteText(TMP_Text objetoTMP, int cantJugador, int cantRequerida) {
        objetoTMP.text = cantJugador.ToString() + "/" + cantRequerida.ToString();
        if (cantJugador < cantRequerida) objetoTMP.color = Color.red;
        else objetoTMP.color = Color.green;
    }

    private void Cocinar() {
        ClearIngredientes();
        btnReceta1.gameObject.SetActive(false);
        btnReceta2.gameObject.SetActive(false);
        btnReceta3.gameObject.SetActive(false);
        btnReceta4.gameObject.SetActive(false);
        btnSpam.gameObject.SetActive(true);

        script.CambiarEstado(true);
        cocinando = true;
    }

    //EL BOTÓN SPAM SIRVE PARA PREPARAR UN PLATO O PARA RECOGERLO CUANDO YA ESTÁ LISTO
    private void Spam() {
        if (recogerPlato) {
            if (script.IsPlayerInZone() && playerInventory.SePuedeCogerPlato()) { //SI EL JUGADOR ESTA EN ZONA Y NO TIENE COGIDO OTRO OBJETO
                playerInventory.CogerPlato(listaRecetas[recetaElegida]);
                ResetCanvas();
            }
        } else { //SPAMEAR BOTÓN PARA PREPARAR EL PLATO
            if (spamSlider.value < 100) spamSlider.value += spamSlider.maxValue / listaRecetas[recetaElegida].clicsPreparacion;
            if (spamSlider.value >= 100 && timerSlider.value < 100) {
                if (script.CanBurn()) {}//Recetas que pueden quemarse
                else {
                    PlatoTerminado();
                }
            }
        }
    }

    public void PlatoTerminado() {
        timerSlider.value = 0;
        spamSlider.value = 0;
        cocinando = false;
        recogerPlato = true;

        btnThrow.gameObject.SetActive(true);
        QuitarIngredientes(recetaElegida);
    }

    //QUITAR INGREDIENTES DEL INVENTARIO DEL JUGADOR
    public void QuitarIngredientes(int posicionReceta) {
        Dictionary<Ingrediente, int> DictIngredientes = listaRecetas[posicionReceta].ingredientes;
        foreach (KeyValuePair<Ingrediente, int> par in DictIngredientes) playerInventory.QuitarIngrediente(par.Key, par.Value);
    }

    //DEVOLVER AL CANVAS A SU ESTADO ORIGINAL
    public void ResetCanvas() {
        btnReceta1.gameObject.SetActive(true);
        btnReceta2.gameObject.SetActive(true);
        btnReceta3.gameObject.SetActive(true);
        btnReceta4.gameObject.SetActive(true);
        btnSpam.gameObject.SetActive(false);
        btnThrow.gameObject.SetActive(false);
        recetaElegida = -1;
        recetaAnterior = -2;
        ClearIngredientes();
        timerSlider.value = 0;
        spamSlider.value = 0;
        cocinando = false;
        recogerPlato = false;
        script.CambiarEstado(false);
    }
}
