using UnityEngine;

/// <summary>
/// The HUD subscribes to WeaponManager events and refreshes when needed.
/// </summary>
public class HUDController : MonoBehaviour, WeaponHUD
{
    [Header("Link to player weapon manager")]
    public WeaponManager weaponManager;

    void OnEnable()
    {
        if (weaponManager != null)
        {
            weaponManager.OnWeaponInfoUpdated += OnWeaponInfoUpdated;
            weaponManager.OnActiveWeaponChanged += OnActiveWeaponChanged;
        }
    }

    void OnDisable()
    {
        if (weaponManager != null)
        {
            weaponManager.OnWeaponInfoUpdated -= OnWeaponInfoUpdated;
            weaponManager.OnActiveWeaponChanged -= OnActiveWeaponChanged;
        }
    }

    private void OnActiveWeaponChanged(Gun gun, WeaponSlot slot)
    {
        // Update any active weapon visuals
        UpdateWeaponDisplay(gun.GetInfo(), slot);
    }

    private void OnWeaponInfoUpdated(WeaponInfo info, WeaponSlot slot)
    {
        UpdateWeaponDisplay(info, slot);
    }

    public void UpdateWeaponDisplay(WeaponInfo info, WeaponSlot slot)
    {
       
        Debug.Log($"HUD update for {slot}: {info.weaponName} | mag {info.currentMagazine} | reserve {info.ammoReserve} | state {info.state}");
    }
}
