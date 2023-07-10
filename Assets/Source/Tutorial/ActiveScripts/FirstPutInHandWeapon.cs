using System;
using UnityEngine;

public class FirstPutInHandWeapon : MonoBehaviour
{
    [SerializeField] private TutorialStage _tutorialStage;
    [SerializeField] private WeaponHandler _weaponHandler;

    private void OnEnable()
    {
        _tutorialStage.Entered += OnTutorialStageEntered;
    }

    private void OnDisable()
    {
        _tutorialStage.Entered -= OnTutorialStageEntered;
    }

    private void OnTutorialStageEntered()
    {
        _weaponHandler.ChangeWeaponTo(WeaponType.Hammer);
        _weaponHandler.CurrentWeapon.PutInHand(false);
    }
}
