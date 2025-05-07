using UnityEngine;

public enum PlateState { Clean, Dirty }

public class Plate : MonoBehaviour
{
    public PlateState state = PlateState.Clean;

    public void Use()
    {
        state = PlateState.Dirty;
        // Cambiar sprite o color para reflejar estado
    }

    public void Clean()
    {
        state = PlateState.Clean;
    }
}
