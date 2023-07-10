using System;
using System.Collections;
using UnityEngine;

public class FirstRepairBuildingStage : HiglightingUIStage
{
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Transform _rootForHandClick;
    [SerializeField] private Vector3 _offsetForHandClick;
    [SerializeField] private RepairButton _repairButton;
    [SerializeField] private Vector3 _offsetForUIOutline;

    protected override void OnEnter()
    {
        base.OnEnter();
        _handClickView.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetSiblingIndex(0);
        _uIOutline.GetComponent<RectTransform>().anchoredPosition = _offsetForUIOutline;
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _handClickView.gameObject.SetActive(true);
        _uIOutline.gameObject.SetActive(true);
        _repairButton.Clicked += OnRepairClicked;
    }

    protected override void OnExit()
    {
        base.OnExit();
        _handClickView.transform.SetParent(transform);
        _handClickView.gameObject.SetActive(false);
        _uIOutline.transform.SetParent(transform);
        _uIOutline.gameObject.SetActive(false);
    }

    private void OnRepairClicked() => Complete();
}
