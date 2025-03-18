using UnityEngine;

namespace PixelGunsPack
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform weaponHolder; // Where the weapon should be attached
        [SerializeField] private Vector3 weaponOffset = new Vector3(0.5f, -0.5f, 1f); // Adjust weapon position
        [SerializeField] private float lerpSpeed = 4f; 

        private void Update()
        {
            if (weaponHolder != null)
            {
                // Smoothly move the weapon to the correct position in front of the player
                weaponHolder.localPosition = Vector3.Lerp(weaponHolder.localPosition, weaponOffset, lerpSpeed * Time.deltaTime);
            }
        }

        public void AttachWeapon(Transform weapon)
        {
            weaponHolder = weapon;
            weapon.SetParent(transform); // Attach weapon to the camera
            weapon.localPosition = weaponOffset; // Set position
            weapon.localRotation = Quaternion.identity; // Reset rotation
        }
    }
}
