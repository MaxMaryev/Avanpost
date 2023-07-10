using System.Collections;
using UnityEngine;

[RequireComponent(typeof(OnTriggerHandler))]
public class Hammer : Weapon, IHandWeapon
{
    //private OnTriggerHandler _triggerHandler;
    //private Coroutine _attackCoroutine;
    //private bool _isAttacking = false;

    public Transform Aim => transform;

    //private void Awake()
    //{
    //    _triggerHandler = GetComponent<OnTriggerHandler>();
    //}

    //private void OnEnable()
    //{
    //    _triggerHandler.TriggerEntered += OnTriggerEntered;
    //}

    //private void OnDisable()
    //{
    //    _triggerHandler.TriggerEntered -= OnTriggerEntered;
    //}

    public override void Accept(IWeaponVisitor weaponVisitor, float damage)
    {
        weaponVisitor.Visit(this, damage);
    }

    public override void Fire()
    {
        //if (_attackCoroutine != null)
        //    return;

        //PlayerAnimationsController.PlayAttackAnimation();
        //_isAttacking = true;
        //_attackCoroutine = StartCoroutine(ReloadAttack());
    }

    public void PutInHand(bool state)
    {
        PlayerAnimationsController.StopReloadAnimation();
        gameObject.SetActive(state);
        PlayerAnimationsController.PlayWeaponSwitch(WeaponType);
    }

    //private IEnumerator ReloadAttack()
    //{
    //    float time = 0;

    //    while (time < AttackCooldown)
    //    {
    //        time += Time.deltaTime;
    //        yield return null;
    //    }

    //    _attackCoroutine = null;
    //    _isAttacking = false;
    //}

    //private void OnTriggerEntered(Collider other)
    //{
    //    if (_isAttacking == false)
    //        return;

    //    if (other.TryGetComponent(out IWeaponVisitor weaponVisitor))
    //    {
    //        Accept(weaponVisitor, MaxDamage);
    //    }
    //}
}
