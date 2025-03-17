using UnityEngine;

public class CameraKitchen : MonoBehaviour
{
    public Transform jugador;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float velocidadSeguimiento = 5f;
    public float velocidadRotacion = 5f;
    public float margen = 6f;

    void LateUpdate()
    {
        Vector3 posJugador = jugador.position;

        if (posJugador.x < -margen) posJugador.x = -margen;
        else if (posJugador.x > margen) posJugador.x = margen;

        transform.position = Vector3.Lerp(transform.position, posJugador + offset, velocidadSeguimiento * Time.deltaTime);

        if (posJugador.x == -margen || posJugador.x == margen)
        {
            Quaternion rotacionDeseada = Quaternion.LookRotation(jugador.position - transform.position);
            Vector3 euler = rotacionDeseada.eulerAngles;
            euler.x = 20;
            rotacionDeseada = Quaternion.Euler(euler);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, velocidadRotacion * Time.deltaTime);
        } else {
            Quaternion rotacionFija = Quaternion.Euler(20f, 0f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionFija, velocidadRotacion * Time.deltaTime);
        }
    }
}