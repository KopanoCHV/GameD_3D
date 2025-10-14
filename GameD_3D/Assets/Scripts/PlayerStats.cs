using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }
}
