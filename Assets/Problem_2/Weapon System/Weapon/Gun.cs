using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Runtime weapon component. Holds runtime ammo state and exposes events for UI / manager.
/// </summary>
public class Gun : MonoBehaviour
{
    [Header("Config")]
    public WeaponConfig config;

    // Runtime state
    public WeaponState State { get; private set; } = WeaponState.Idle;
    public int CurrentMagazine { get; private set; }
    public int AmmoReserve { get; private set; }

    // Events
    public event Action<Gun> OnAmmoChanged;
    public event Action<Gun> OnStateChanged;
    public event Action<Gun> OnFired; // can be used for paritcles or sounds etc 

    private float fireCooldown = 0f;

    void Awake()
    {
        if (config != null)
        {
            CurrentMagazine = config.magazineSize;
            AmmoReserve = config.ammoReserve;
        }
        else
        {
            CurrentMagazine = 0;
            AmmoReserve = 0;
        }
    }

    void Update()
    {
        if (fireCooldown > 0f)
            fireCooldown -= Time.deltaTime;
    }

    /// <summary>
    /// Try to fire once. Returns true if a shot was fired.
    /// </summary>
    public bool TryFire()
    {
        if (config == null) return false;
        if (State == WeaponState.Reloading) return false;
        if (fireCooldown > 0f) return false;

        if (CurrentMagazine <= 0)
        {
            // auto trigger reload if empty
            StartReload();
            return false;
        }

        DoFire();
        return true;
    }

    private void DoFire()
    {
        State = WeaponState.Firing;
        OnStateChanged?.Invoke(this);

        // reduce ammo
        CurrentMagazine--;
        OnAmmoChanged?.Invoke(this);

        // fire cooldown
        fireCooldown = 1f / Mathf.Max(0.0001f, config.fireRate);

        // spawn projectile (placeholder - replace with pooling)
        if (config.projectilePrefab != null)
        {
            Vector3 spawnPos = transform.position + transform.TransformDirection(config.muzzleOffset);
            Instantiate(config.projectilePrefab, spawnPos, transform.rotation);
        }

        OnFired?.Invoke(this);

        // go back to idle
        State = WeaponState.Idle;
        OnStateChanged?.Invoke(this);
    }

    public void StartReload()
    {
        if (State == WeaponState.Reloading) return;
        if (CurrentMagazine >= config.magazineSize) return;
        if (AmmoReserve <= 0) return;

        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        State = WeaponState.Reloading;
        OnStateChanged?.Invoke(this);

        yield return new WaitForSeconds(config.reloadTime);

        int needed = config.magazineSize - CurrentMagazine;
        int taken = Mathf.Min(needed, AmmoReserve);
        CurrentMagazine += taken;
        AmmoReserve -= taken;

        State = WeaponState.Idle;
        OnStateChanged?.Invoke(this);
        OnAmmoChanged?.Invoke(this);
    }

    /// <summary>
    /// Add ammo 
    /// </summary>
    public void AddAmmo(int amount)
    {
        AmmoReserve += amount;
        OnAmmoChanged?.Invoke(this);
    }

    /// <summary>
    /// Helper to get WeaponInfo for UI
    /// </summary>
    public WeaponInfo GetInfo()
    {
        string name = config != null ? config.weaponName : "None";
        return new WeaponInfo(name, CurrentMagazine, AmmoReserve, State);
    }
}
