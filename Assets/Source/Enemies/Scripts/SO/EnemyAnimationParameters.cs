using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyHitAnimationParameters", menuName = "SO/EnemyHitAnimationParameters", order = 51)]
public class EnemyAnimationParameters : ScriptableObject
{
    [SerializeField] private List<HitAnimationParametes> _hitAnimationParametes = new List<HitAnimationParametes>();
    [SerializeField] private int _attackValueMin;
    [SerializeField] private int _attackValueMax;
    [SerializeField] private int _fellValueMin;
    [SerializeField] private int _fellValueMax;
    [SerializeField] private int _MoveValueMin;
    [SerializeField] private int _MoveValueMax;
    [SerializeField] private int _DeathValueMin;
    [SerializeField] private int _DeathValueMax;

    public int RandomValueDeath => Random.Range(_DeathValueMin, _DeathValueMax);
    public int RandomValueMove => Random.Range(_MoveValueMin, _MoveValueMax);
    public int RandomValueAttack => Random.Range(_attackValueMin, _attackValueMax);
    public int RandomValueFell => Random.Range(_fellValueMin, _fellValueMax);

    public bool TryGet(WeaponType weaponType, out HitAnimationParametes animationParametes)
    {
        foreach (var param in _hitAnimationParametes)
        {
            if (param.WeaponType == weaponType)
            {
                animationParametes = param;
                return true;
            }
        }

        animationParametes = null;
        return false;
    }
}
