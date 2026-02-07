using System;
using UnityEngine;

/// <summary>
/// Main Centralized Manager
/// </summary>
public class WeaponManager : MonoBehaviour
{
    public Gun primary1Prefab;
    public Gun primary2Prefab;
    public Gun secondaryPrefab;

    // instantiated runtime instances 
    private Gun primary1;
    private Gun primary2;
    private Gun secondary;

    private Gun activeWeapon;

    // UI subscriptions EVENTS
    public event Action<Gun, WeaponSlot> OnWeaponEquipped;      // fired when equip occurs
    public event Action<Gun, WeaponSlot> OnActiveWeaponChanged; // fired when active weapon switches
    public event Action<WeaponInfo, WeaponSlot> OnWeaponInfoUpdated; // forwarded when ammo/state changes

    void Start()
    {
        // optionally auto-equip prefabs at start 
        if (primary1Prefab != null) Equip(primary1Prefab, WeaponSlot.Primary1);
        if (primary2Prefab != null) Equip(primary2Prefab, WeaponSlot.Primary2);
        if (secondaryPrefab != null) Equip(secondaryPrefab, WeaponSlot.Secondary);
    }

    public void Equip(Gun prefab, WeaponSlot slot)
    {
        if (prefab == null) return;

        // instantiate as child for organization
        Gun instance = Instantiate(prefab, transform);
        instance.gameObject.name = prefab.gameObject.name + "_" + slot;

        // route to slot
        switch (slot)
        {
            case WeaponSlot.Primary1:
                if (primary1 != null) Destroy(primary1.gameObject);
                primary1 = instance;
                break;
            case WeaponSlot.Primary2:
                if (primary2 != null) Destroy(primary2.gameObject);
                primary2 = instance;
                break;
            case WeaponSlot.Secondary:
                if (secondary != null) Destroy(secondary.gameObject);
                secondary = instance;
                break;
        }

        // subscribe for runtime updates (ammo/state)
        instance.OnAmmoChanged += HandleWeaponAmmoChanged;
        instance.OnStateChanged += HandleWeaponStateChanged;

        OnWeaponEquipped?.Invoke(instance, slot);

        // If no active weapon, set to this
        if (activeWeapon == null) SetActive(instance, slot);
        else instance.gameObject.SetActive(false);
    }

    private void HandleWeaponAmmoChanged(Gun gun)
    {
        WeaponSlot slot = GetSlotOf(gun);
        OnWeaponInfoUpdated?.Invoke(gun.GetInfo(), slot);
    }

    private void HandleWeaponStateChanged(Gun gun)
    {
        WeaponSlot slot = GetSlotOf(gun);
        OnWeaponInfoUpdated?.Invoke(gun.GetInfo(), slot);
    }

    private WeaponSlot GetSlotOf(Gun gun)
    {
        if (gun == primary1) return WeaponSlot.Primary1;
        if (gun == primary2) return WeaponSlot.Primary2;
        if (gun == secondary) return WeaponSlot.Secondary;
        return WeaponSlot.Primary1; // fallback
    }

    /// <summary>
    /// Switch active weapon. This disables previous weapon GameObject and enables the new one.
    /// </summary>
    public void SetActive(WeaponSlot slot)
    {
        Gun toActivate = GetWeapon(slot);
        if (toActivate == null) return;

        // disable previous
        if (activeWeapon != null)
            activeWeapon.gameObject.SetActive(false);

        // activate new
        activeWeapon = toActivate;
        activeWeapon.gameObject.SetActive(true);

        OnActiveWeaponChanged?.Invoke(activeWeapon, slot);
        // notify UI of current state & ammo
        OnWeaponInfoUpdated?.Invoke(activeWeapon.GetInfo(), slot);
    }

    ///  set active by Gun instance
    /// </summary>
    public void SetActive(Gun gun, WeaponSlot assumedSlot)
    {
        if (gun == null) return;
        if (activeWeapon != null)
            activeWeapon.gameObject.SetActive(false);

        activeWeapon = gun;
        activeWeapon.gameObject.SetActive(true);
        OnActiveWeaponChanged?.Invoke(activeWeapon, assumedSlot);
        OnWeaponInfoUpdated?.Invoke(activeWeapon.GetInfo(), assumedSlot);
    }

    public Gun GetWeapon(WeaponSlot slot)
    {
        switch (slot)
        {
            case WeaponSlot.Primary1: return primary1;
            case WeaponSlot.Primary2: return primary2;
            case WeaponSlot.Secondary: return secondary;
        }
        return null;
    }

    /// <summary>
    /// Called from input or Player controller
    /// </summary>
    public void FireActive()
    {
        if (activeWeapon == null) return;
        activeWeapon.TryFire();
    }

    public void StartReloadActive()
    {
        if (activeWeapon == null) return;
        activeWeapon.StartReload();
    }

    public void SwitchToNextPrimary()
    {
        // simple cycle: P1 -> P2 -> P1 (ignores secondary)
        if (activeWeapon == primary1 && primary2 != null) SetActive(WeaponSlot.Primary2);
        else if (activeWeapon == primary2 && primary1 != null) SetActive(WeaponSlot.Primary1);
        else if (primary1 != null) SetActive(WeaponSlot.Primary1);
    }
}
