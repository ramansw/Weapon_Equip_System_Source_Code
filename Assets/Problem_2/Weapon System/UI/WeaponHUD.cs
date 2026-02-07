public interface WeaponHUD
{
    // Called whenever HUD should refresh an individual weapon display
    void UpdateWeaponDisplay(WeaponInfo info, WeaponSlot slot);
}
