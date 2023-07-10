using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBuyWeaponStage : NavigationStage
{
    [SerializeField] private WalletPresenter _walletPresenter;
    [SerializeField] private Player _player;
    [SerializeField] private float _maxDistanceToTarget;
    [SerializeField] private Night _night;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private WeaponPanelSlots _weaponPanelSlots;
    [SerializeField] private WeaponSlotsDatasSO _weaponSlotsDatasSO;
    [SerializeField] private SubTutorial _subTutorial;
    [SerializeField] private TutorialStage _targetStageForSkip;

    private bool _isReadyBuyWeapon => GetDistanceToTarget(_player.transform.position) <= _maxDistanceToTarget &&
                                      _walletPresenter.Value >= _slotData.JunkNeeded && 
                                      _night.IsTimeInRange(_dayCycleManager.CurrentNormalizedTime) == false;
    private WeaponSlotData _slotData;
    private Coroutine _coroutine;

    protected override void OnEnter()
    {
        StartCoroutine(WaitFrame());
    }

    private IEnumerator WaitFrame()
    {
        yield return new WaitForEndOfFrame();

        CheckWeaponPlayer();
    }

    private void CheckWeaponPlayer()
    {
        _slotData = _weaponSlotsDatasSO.SlotsDatas[0];

        foreach (var weapon in _weaponPanelSlots.GetAvailableWeapons())
        {
            if (weapon == _slotData.WeaponType)
            {
                _subTutorial.SkipStageTo(_targetStageForSkip);
                return;
            }
        }

        if (_isReadyBuyWeapon)
            base.OnEnter();
        else
            _coroutine = StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        while (_isReadyBuyWeapon == false)
        {
            yield return new WaitForSeconds(2f);
            yield return null;
        }

        base.OnEnter();
    }
}
