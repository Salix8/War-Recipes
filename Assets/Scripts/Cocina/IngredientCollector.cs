using UnityEngine;

public class IngredientCollector : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float attractionDistance = 10f;
    [SerializeField] private float collectDistance = 1.5f;
    [SerializeField] private float moveSpeed = 1f;

    private Ingredient ingrediente;
    private Transform player;
    private bool shouldMoveToPlayer = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("[IngredientCollector] No se ha encontrado un jugador con la tag 'Player'.");
        }

        IngredientInstance instancia = GetComponent<IngredientInstance>();
        if (instancia != null)
        {
            ingrediente = instancia.GetIngredient();
        }
        else
        {
            Debug.LogWarning("[IngredientCollector] No se encontro un Ingredient valido en este objeto.");
        }
    }

    void Update()
    {
        if (player == null || ingrediente == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= collectDistance)
        {
            ItemManager.AnyadirIngrediente(ingrediente);
            Destroy(gameObject);
        }
        else if (distance <= attractionDistance)
        {
            shouldMoveToPlayer = true;
        }

        if (shouldMoveToPlayer)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
