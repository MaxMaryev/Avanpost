using UnityEngine;

public class Shotgun : FiringWeapon, IHandWeapon
{
    [SerializeField] private Transform _aim;

    public Transform Aim => _aim;

    public override void Accept(IWeaponVisitor weaponVisitor, float damage)
    {
        weaponVisitor.Visit(this, damage);
    }

    public void PutInHand(bool state) 
    {
        PlayerAnimationsController.StopReloadAnimation();
        gameObject.SetActive(state);
        PlayerAnimationsController.PlayWeaponSwitch(WeaponType);
    }
}