using UnityEngine;

namespace PixelGunsPack
{
    public class GunSway : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField] private float swayAmount = 0.05f;
        [SerializeField] private float smoothness = 2f;

        private Vector3 initialPosition;

        private void Start()
        {
            initialPosition = transform.localPosition;
        }

        private void Update()
        {
            float moveX = Input.GetAxis("Mouse X") * swayAmount;
            float moveY = Input.GetAxis("Mouse Y") * swayAmount;

            Vector3 finalPosition = new Vector3(initialPosition.x + moveX, initialPosition.y + moveY, initialPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition, Time.deltaTime * smoothness);
        }
    }
}
