using System;
using UnityEngine;

public class ClickButtonBuyObstacleStage : HiglightingUIStage
{
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Transform _rootForHandClick;
    [SerializeField] private Vector3 _offsetForHandClick;
    [SerializeField] private ButtonCraftItem _craftButton;

    protected override void OnEnter()
    {
        base.OnEnter();

        _handClickView.transform.SetParent(_rootForHandClick, false);
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _uIOutline.transform.SetParent(_rootForHandClick, false);
        _uIOutline.gameObject.SetActive(true);

        _craftButton.Clicked += OnCraftButtonClicked;
    }

    private void OnCraftButtonClicked()
    {
        _craftButton.Clicked -= OnCraftButtonClicked;

        Complete();
    }

    protected override void OnExit()
    {
        base.OnExit();

        _handClickView.transform.SetParent(transform);
        _uIOutline.transform.SetParent(transform);
        _handClickView.gameObject.SetActive(false);
        _uIOutline.gameObject.SetActive(false);
    }
}
