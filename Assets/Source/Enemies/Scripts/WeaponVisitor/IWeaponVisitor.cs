using UnityEngine;

public interface IWeaponVisitor 
{
    public GameObject GameObject { get; }
    public void Visit(Pistol pistol, float damage);
    public void Visit(Shotgun shotgun,float damage);
    public void Visit(SniperRifle sniperRifle, float damage);
    public void Visit(AK aK, float damage);

    public void Visit(Minigun minigun, float damage);
    public void Visit(Flamethrower minigun, float damage);
    public void Visit(ShotgunTrap shotgunTrap, float damage);
    public void Visit(TwoBarrelsGun shotgunTrap, float damage);
    public void Visit(Hammer hammer, float damage);

    public void Visit(Mine mine, float damage);
}
