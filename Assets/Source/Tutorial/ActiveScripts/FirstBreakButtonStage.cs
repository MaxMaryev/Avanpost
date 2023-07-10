using System;
using UnityEngine;

public class FirstBreakButtonStage : HiglightingUIStage
{
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Transform _rootForHandClick;
    [SerializeField] private Vector3 _offsetForHandClick;
    [SerializeField] private BreakButton _breakButton;
    [SerializeField] private Vector3 _offsetForUIOutline;

    protected override void OnEnter()
    {
        base.OnEnter();
        _handClickView.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetSiblingIndex(0);
        _uIOutline.GetComponent<RectTransform>().anchoredPosition = _offsetForUIOutline;
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _uIOutline.gameObject.SetActive(true);
        _breakButton.Clicked += OnBreakButtonClicked;
    }

    private void OnBreakButtonClicked()
    {
        _breakButton.Clicked -= OnBreakButtonClicked;
        Complete();
    }

    protected override void OnExit()
    {
        base.OnExit();
        _handClickView.transform.SetParent(transform);
        _handClickView.gameObject.SetActive(false);
        _uIOutline.transform.SetParent(transform);
        _uIOutline.gameObject.SetActive(false);
    }
}
