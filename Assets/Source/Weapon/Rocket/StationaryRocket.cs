using System.Collections;
using UnityEngine;

public class StationaryRocket : FiringWeapon
{
    //[SerializeField] private _selectedSlot _selectedSlot;
    //[SerializeField] private Animator _animator;
    //[SerializeField] private OnTriggerHandler _reloadTrigger;
    //[SerializeField] private BoxCollider _trapAreaCollider;

    //private Coroutine _reloadCoroutine;
    //private bool _isBroken;

    //private bool _isPlayerNear;

    //private void OnEnable()
    //{
    //    _reloadTrigger.TriggerEntered += OnReloadTriggerEntered;
    //    _reloadTrigger.TriggerExited += OnReloadTriggerExited;
    //    _selectedSlot.SafetyMarginChanged += OnSafetyMarginChanged;
    //}

    //private void OnDisable()
    //{
    //    _reloadTrigger.TriggerEntered -= OnReloadTriggerEntered;
    //    _reloadTrigger.TriggerExited -= OnReloadTriggerExited;
    //    _selectedSlot.SafetyMarginChanged -= OnSafetyMarginChanged;
    //}

    //public void Init(WeaponHandler weaponHandler) => WeaponHandler = weaponHandler;

    //public override void Accept(IWeaponVisitor weaponVisitor, float damage, bool isCrit)
    //{
    //    weaponVisitor.Visit(this, damage, isCrit);
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    _isPlayerNear = true;

    //    if (_isBroken)
    //        return;

    //    if (IsReloaded == false)
    //        return;

    //    if (other.TryGetComponent(out Enemy enemy))
    //        Fire();
    //}

    //private void OnReloadTriggerEntered(Collider other)
    //{
    //    if (other.GetComponent<Player>())
    //    {
    //        _isPlayerNear = true;

    //        if (_reloadCoroutine == null)
    //            _reloadCoroutine = StartCoroutine(Reloading());
    //    }
    //}
    //private void OnReloadTriggerExited(Collider other)
    //{
    //    if (other.GetComponent<Player>())
    //    {
    //        _isPlayerNear = false;

    //        if (_reloadCoroutine != null)
    //        {
    //            StopCoroutine(_reloadCoroutine);
    //            _reloadCoroutine = null;
    //        }
    //    }
    //}

    //private IEnumerator Reloading()
    //{
    //    WaitForSeconds waitForSeconds = new WaitForSeconds(DefaultReloadDuration);
    //    while (_isPlayerNear)
    //    {
    //        yield return waitForSeconds;

    //        if (CurrentBulletsCount < ClipCapacity)
    //        {
    //            CurrentBulletsCount++;
    //            BlinkTrapAreaCollider();
    //        }
    //    }

    //    _reloadCoroutine = null;

    //    void BlinkTrapAreaCollider()
    //    {
    //        _trapAreaCollider.enabled = false;
    //        _trapAreaCollider.enabled = true;
    //    }
    //}

    //public override void Fire()
    //{
    //    if (IsReloaded == false)
    //        return;

    //    _animator.Play(Constants.AnimatorStates.Fire);
    //    Bullet.Play();
    //    ShotEffect.Play();
    //    CurrentBulletsCount--;
    //}

    //private void OnSafetyMarginChanged(float maxValue, float currentValue)
    //{
    //    if (currentValue >= maxValue * 0.9f)
    //        _isBroken = false;
    //    else
    //        _isBroken = true;
    //}
}
