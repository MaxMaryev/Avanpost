using UnityEngine;

public class Turret : Building
{
    [SerializeField] private BulletAmountView _bulletAmountView;

    [field: SerializeField] public FiringWeapon FiringWeapon { get; private set; }

    protected override void OnStartDestroying()
    {
        _bulletAmountView.gameObject.SetActive(false);
    }
}