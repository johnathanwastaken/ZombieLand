using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the bullet

    private void OnTriggerEnter(Collider other)
    {
        // ✅ Check if we hit an enemy
        ReactiveTarget target = other.GetComponent<ReactiveTarget>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        // ✅ Destroy the bullet upon impact
        Destroy(gameObject);
    }
}
