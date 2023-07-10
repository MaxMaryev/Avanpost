using UnityEngine;

public interface IHandWeapon
{
    public Transform Aim { get; }
    public WeaponHandler WeaponHandler { get; }
    public float ShotDistance { get; }
    public WeaponType WeaponType { get; }

    public void Fire();
    public void PutInHand(bool state);
}
