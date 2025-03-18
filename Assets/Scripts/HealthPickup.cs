using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 3; // Amount of health restored

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();

        if (player != null)
        {
            // Restore health and destroy the pickup
            player.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
