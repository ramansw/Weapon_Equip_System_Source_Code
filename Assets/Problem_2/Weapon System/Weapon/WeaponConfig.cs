using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Weapons/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    public string weaponName = "New Weapon";
    public int magazineSize = 30;
    public int ammoReserve = 90;        
    public float fireRate = 10f;        // rounds per second
    public float reloadTime = 2f;       // seconds
    public float damage = 20f;
    public bool isAutomatic = true;
    public GameObject projectilePrefab; // optional projectile prefab
    public Vector3 muzzleOffset = Vector3.forward * 0.5f;
}
