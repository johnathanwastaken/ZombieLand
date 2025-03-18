using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet that will be fired
    public Transform firePoint; // Where bullets spawn
    public float bulletSpeed = 20f; // Speed of bullets
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError($"Weapon {gameObject.name} is missing bulletPrefab or firePoint!");
            return;
        }

        // Spawn a bullet from the firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        // Apply force to move the bullet
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }

        // Play gunshot sound
        audioSource.PlayOneShot(audioSource.clip);

        // Destroy bullet after 5 seconds to prevent clutter
        Destroy(bullet, 5f);
    }
}
