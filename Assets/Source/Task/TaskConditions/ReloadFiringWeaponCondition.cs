using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReloadFiringWeaponCondition : MonoBehaviour, ITaskCondition, ICountable
{
    [SerializeField] private ContainersHandler _containersHandler;

    private int _numberOfWeaponsRequiringReloading;
    private int _currentNumberWeaponReloaded;
    private List<FiringWeapon> _firingWeapons = new List<FiringWeapon>(0);

    public int TargetNumber => _numberOfWeaponsRequiringReloading;

    public int CurrentNumber => _currentNumberWeaponReloaded;

    public event Action Updated;

    private void OnEnable()
    {
        foreach (var weapon in _firingWeapons)
            weapon.Reloaded += OnFiringWeaponReloaded;
    }

    private void OnDisable()
    {
        foreach (var weapon in _firingWeapons)
            weapon.Reloaded -= OnFiringWeaponReloaded;
    }

    public bool CompleteConditionMet()
    {
        return _currentNumberWeaponReloaded >= _numberOfWeaponsRequiringReloading;
    }

    public bool FailConditionMet()
    {
        return false;
    }

    public bool StartConditionMet()
    {
        _firingWeapons.Clear();
        _firingWeapons.AddRange(_containersHandler.GetComponentsInChildren<FiringWeapon>());
        _numberOfWeaponsRequiringReloading = _firingWeapons.Where(x => x.CurrentBulletsCount < x.ClipCapacity).Count();
        _currentNumberWeaponReloaded = _firingWeapons.Where(x => x.CurrentBulletsCount >= x.ClipCapacity).Count();
        return _currentNumberWeaponReloaded < _numberOfWeaponsRequiringReloading;
    }

    private void OnFiringWeaponReloaded(FiringWeapon firingWeapon)
    {
        firingWeapon.Reloaded -= OnFiringWeaponReloaded;
        _currentNumberWeaponReloaded++;
        Updated?.Invoke();
    }
}
