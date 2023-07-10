using System;
using System.Collections;
using UnityEngine;

public class ShowWeaponSubMenuStage : HiglightingUIStage
{
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private CraftSubMenuButton _craftSubMenuButton;
    [SerializeField] private Vector3 _offsetForHandClick;

    protected override void OnEnter()
    {
        base.OnEnter();

        StartCoroutine(WaitOneFrame());
    }

    protected override void OnExit()
    {
        base.OnExit();

        _handClickView.transform.SetParent(transform);
        _handClickView.gameObject.SetActive(false);
        _uIOutline.transform.SetParent(transform);
        _uIOutline.gameObject.SetActive(false);
    }

    private IEnumerator WaitOneFrame()
    {
        yield return new WaitForEndOfFrame();

        _handClickView.transform.SetParent(_craftSubMenuButton.transform, false);
        _uIOutline.transform.SetParent(_craftSubMenuButton.transform, false);
        _uIOutline.transform.SetSiblingIndex(0);
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _uIOutline.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        _craftSubMenuButton.Clicked += OnSubMenuClicked;
    }

    private void OnSubMenuClicked(CraftSubMenuButton craftSubMenuButton)
    {
        craftSubMenuButton.Clicked -= OnSubMenuClicked;

        Complete();
    }
}
