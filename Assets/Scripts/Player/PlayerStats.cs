using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public float regenPerSecond = 5f;
}
