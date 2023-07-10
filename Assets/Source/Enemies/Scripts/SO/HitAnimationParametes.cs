using UnityEngine;

[System.Serializable]
public class HitAnimationParametes 
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private int _actionMin;
    [SerializeField] private int _actionMax;

    public WeaponType WeaponType => _weaponType;
    public int RandomAction => Random.Range(_actionMin, _actionMax);
}
