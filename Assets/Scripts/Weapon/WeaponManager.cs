using UnityEngine;
using PixelGunsPack; // If using CameraController

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weaponPrefabs;
    private GameObject[] weapons;
    private int currentWeaponIndex = 0;

    private Transform weaponHolder; // The player's weapon mount point

    // ✅ Custom positions for each weapon (adjust as needed)
    private Vector3[] weaponPositions =
    {
        new Vector3(-0.09f, -0.04f, -0.06f), // Pistol Position
        new Vector3(0.42f, -0.26f, 0.6f), // SMG Position
        new Vector3(0.6f, -0.21f, 0.63f)  // AR Position
    };

    private Vector3[] weaponRotations =
    {
        new Vector3(0, 0, 0), // Pistol Rotation
        new Vector3(0, 5, 0), // SMG Rotation
        new Vector3(0, 10, 0) // AR Rotation
    };

    void Start()
    {
        weaponHolder = Camera.main?.transform;
        if (weaponHolder == null)
        {
            Debug.LogError("Main Camera not found! Make sure your player has a Main Camera.");
            return;
        }

        weapons = new GameObject[weaponPrefabs.Length];

        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            weapons[i] = Instantiate(weaponPrefabs[i], weaponHolder);
            weapons[i].SetActive(i == 0); // Activate only the first weapon

            // Apply different positions and rotations
            weapons[i].transform.localPosition = weaponPositions[i];
            weapons[i].transform.localRotation = Quaternion.Euler(weaponRotations[i]);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectWeapon(2);

        if (Input.GetMouseButtonDown(0) && weapons[currentWeaponIndex] != null)
        {
            Weapon currentWeapon = weapons[currentWeaponIndex].GetComponent<Weapon>();
            if (currentWeapon != null)
            {
                currentWeapon.Shoot();
            }
        }
    }

    void SelectWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        // Apply the custom position and rotation for the new weapon
        weapons[index].transform.localPosition = weaponPositions[index];
        weapons[index].transform.localRotation = Quaternion.Euler(weaponRotations[index]);

        currentWeaponIndex = index;
    }
}
