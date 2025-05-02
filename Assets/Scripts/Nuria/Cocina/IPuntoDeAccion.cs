using UnityEngine;

public interface IPuntoDeAccion
{
    string GetName();
    bool CanBurn();
    bool IsPlayerInZone();
    void CambiarEstado(bool activo);
}
