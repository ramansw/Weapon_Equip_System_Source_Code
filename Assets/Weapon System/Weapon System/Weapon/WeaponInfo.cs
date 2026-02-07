// Small lightweight struct used to send ammo/state data to UI
public struct WeaponInfo
{
    public string weaponName;
    public int currentMagazine;
    public int ammoReserve;
    public WeaponState state;

    public WeaponInfo(string name, int mag, int reserve, WeaponState s)
    {
        weaponName = name;
        currentMagazine = mag;
        ammoReserve = reserve;
        state = s;
    }
}
