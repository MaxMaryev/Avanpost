using System;
using System.Collections;
using UnityEngine;

public class WeaponSlotStage : HiglightingUIStage
{
    [SerializeField] private WeaponPanel _weaponPanel;
    [SerializeField] private WeaponPanelSlot _weaponPanelSlot;
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Transform _rootForHandClick;
    [SerializeField] private Vector3 _offsetForHandClick;
    [SerializeField] private WeaponHandler _weaponHandler;
    [SerializeField] private PlayerMover _playerMover;

    public override event Action<TutorialStage> Completed;

    private void Start()
    {
        StartCoroutine(WaitFrame());
    }

    protected override void OnEnter()
    {
        _playerMover.enabled = false;
        _weaponPanel.gameObject.SetActive(true);
        base.OnEnter();
        _handClickView.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetSiblingIndex(0);
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _uIOutline.gameObject.SetActive(true);
        _weaponPanelSlot.Selected += OnWeaponPanelSlotSelected;
    }

    private void OnWeaponPanelSlotSelected(WeaponType weaponType, bool isReward)
    {
        _weaponPanelSlot.Selected -= OnWeaponPanelSlotSelected;
        Completed?.Invoke(this);
    }

    protected override void OnExit()
    {
        base.OnExit();
        _playerMover.enabled = true;
        _handClickView.transform.SetParent(transform);
        _handClickView.gameObject.SetActive(false);
        _uIOutline.transform.SetParent(transform);
        _uIOutline.gameObject.SetActive(false);
    }

    private IEnumerator WaitFrame()
    {
        yield return new WaitForEndOfFrame();
        _weaponPanel.gameObject.SetActive(false);
    }
}
