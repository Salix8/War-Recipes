using UnityEngine;
using System.Collections.Generic;

public class Recetario : MonoBehaviour
{
    public List<Receta> recetas = new List<Receta>();
    public List<Ingrediente> ingredientes = new List<Ingrediente>();

    void Awake()
    {
        CrearListas();
    }

    private void CrearListas() {
        // LISTA INGREDIENTES
        Ingrediente miel = new Ingrediente(0, "Miel", Resources.Load<Sprite>("Iconos/Miel"));
        ingredientes.Add(miel);

        Ingrediente jaleaReal = new Ingrediente(1, "Jalea Real", Resources.Load<Sprite>("Iconos/JaleaReal"));
        ingredientes.Add(jaleaReal);

        Ingrediente cactus = new Ingrediente(2, "Cactus", Resources.Load<Sprite>("Iconos/Cactus"));
        ingredientes.Add(cactus);

        Ingrediente tuna = new Ingrediente(3, "Tuna", Resources.Load<Sprite>("Iconos/Tuna"));
        ingredientes.Add(tuna);

        Ingrediente champinon = new Ingrediente(4, "Champinon", Resources.Load<Sprite>("Iconos/Champinon"));
        ingredientes.Add(champinon);

        Ingrediente esporas = new Ingrediente(5, "Esporas", Resources.Load<Sprite>("Iconos/Esporas"));
        ingredientes.Add(esporas);

        Ingrediente carne = new Ingrediente(6, "Carne", Resources.Load<Sprite>("Iconos/Carne"));
        ingredientes.Add(carne);

        Ingrediente tuetano = new Ingrediente(7, "Tuetano", Resources.Load<Sprite>("Iconos/Tuetano"));
        ingredientes.Add(tuetano);

        Ingrediente gelatina = new Ingrediente(8, "Gelatina", Resources.Load<Sprite>("Iconos/Gelatina"));
        ingredientes.Add(gelatina);

        Ingrediente pescado = new Ingrediente(9, "Pescado", Resources.Load<Sprite>("Iconos/Pescado"));
        ingredientes.Add(pescado);

        Ingrediente huevasDePescado = new Ingrediente(10, "Huevas de Pescado", Resources.Load<Sprite>("Iconos/Huevas"));
        ingredientes.Add(huevasDePescado);

        Ingrediente escamas = new Ingrediente(11, "Escamas", Resources.Load<Sprite>("Iconos/Escamas"));
        ingredientes.Add(escamas);

        Ingrediente tentaculos = new Ingrediente(12, "Tentaculos", Resources.Load<Sprite>("Iconos/Tentaculos"));
        ingredientes.Add(tentaculos);

        Ingrediente aceiteVegetal = new Ingrediente(13, "Aceite Vegetal", Resources.Load<Sprite>("Iconos/AceiteVegetal"));
        ingredientes.Add(aceiteVegetal);

        Ingrediente carneDeAve = new Ingrediente(14, "Carne de Ave", Resources.Load<Sprite>("Iconos/CarneDeAve"));
        ingredientes.Add(carneDeAve);

        Ingrediente huevo = new Ingrediente(15, "Huevo", Resources.Load<Sprite>("Iconos/Huevo"));
        ingredientes.Add(huevo);

        Ingrediente alientoIgneo = new Ingrediente(16, "Aliento Igneo", Resources.Load<Sprite>("Iconos/AlientoIgneo"));
        ingredientes.Add(alientoIgneo);

        // LISTA RECETAS
        // Receta 1 - Hamburguesa
        Dictionary<Ingrediente, int> ingredientesHamburguesa = new Dictionary<Ingrediente, int>();
        ingredientesHamburguesa.Add(carne, 2);
        ingredientesHamburguesa.Add(huevo, 1);
        Receta Hamburguesa = new Receta(0, "Hamburguesa McTroll", "fogones", Resources.Load<Sprite>("Iconos/Hamburguesa"), 1, 5, 5, ingredientesHamburguesa);
        recetas.Add(Hamburguesa);

        // Receta 2 - Chuleta
        Dictionary<Ingrediente, int> ingredientesChuleta = new Dictionary<Ingrediente, int>();
        ingredientesChuleta.Add(carne, 3);
        ingredientesChuleta.Add(miel, 1);
        Receta Chuleta = new Receta(1, "Chuleta", "fogones", Resources.Load<Sprite>("Iconos/Chuleta"), 1, 6, 7, ingredientesChuleta);
        recetas.Add(Chuleta);

        // Receta 3 - Desayuno
        Dictionary<Ingrediente, int> ingredientesDesayuno = new Dictionary<Ingrediente, int>();
        ingredientesDesayuno.Add(huevo, 2);
        ingredientesDesayuno.Add(carne, 1);
        ingredientesDesayuno.Add(miel, 1);
        Receta Desayuno = new Receta(2, "Desayuno Completo", "fogones", Resources.Load<Sprite>("Iconos/Desayuno"), 1, 8, 9, ingredientesDesayuno);
        recetas.Add(Desayuno);

        // Receta 4 - Pulpo
        Dictionary<Ingrediente, int> ingredientesPulpo = new Dictionary<Ingrediente, int>();
        ingredientesPulpo.Add(tentaculos, 3);
        ingredientesPulpo.Add(esporas, 2);
        ingredientesPulpo.Add(aceiteVegetal, 2);
        ingredientesPulpo.Add(alientoIgneo, 1);
        Receta Pulpo = new Receta(3, "Pulpo a la Guerrera", "fogones", Resources.Load<Sprite>("Iconos/Pulpo"), 1, 10, 11, ingredientesPulpo);
        recetas.Add(Pulpo);

        // Receta 5 - Zumo 
        Dictionary<Ingrediente, int> ingredientesZumo = new Dictionary<Ingrediente, int>();
        ingredientesZumo.Add(tuna, 2);
        ingredientesZumo.Add(esporas, 1);
        Receta Zumo = new Receta(4, "Zumo de fruta", "mesa", Resources.Load<Sprite>("Iconos/Zumo"), 1, 3, 5, ingredientesZumo);
        recetas.Add(Zumo);

        // Receta 6 - Sushi 
        Dictionary<Ingrediente, int> ingredientesSushi = new Dictionary<Ingrediente, int>();
        ingredientesSushi.Add(pescado, 2);
        ingredientesSushi.Add(tentaculos, 1);
        ingredientesSushi.Add(huevasDePescado, 1);
        Receta Sushi = new Receta(5, "Sushi sospechoso", "mesa", Resources.Load<Sprite>("Iconos/Zumo"), 1, 7, 7, ingredientesSushi);
        recetas.Add(Sushi);

        // Receta 7 - Tartar 
        Dictionary<Ingrediente, int> ingredientesTartar = new Dictionary<Ingrediente, int>();
        ingredientesTartar.Add(pescado, 2);
        ingredientesTartar.Add(jaleaReal, 1);
        ingredientesTartar.Add(tuna, 2);
        Receta Tartar = new Receta(5, "Tartar de megapescado", "mesa", Resources.Load<Sprite>("Iconos/Zumo"), 1, 8, 9, ingredientesTartar);
        recetas.Add(Tartar);

        // Receta 8 - Ensalada 
        Dictionary<Ingrediente, int> ingredientesEnsalada = new Dictionary<Ingrediente, int>();
        ingredientesEnsalada.Add(cactus, 3);
        ingredientesEnsalada.Add(aceiteVegetal, 1);
        ingredientesEnsalada.Add(carneDeAve, 2);
        ingredientesEnsalada.Add(huevo, 1);
        Receta Ensalada = new Receta(5, "Ensalada", "mesa", Resources.Load<Sprite>("Iconos/Zumo"), 1, 8, 9, ingredientesEnsalada);
        recetas.Add(Ensalada);
    }
}
