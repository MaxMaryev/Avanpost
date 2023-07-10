using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SelectWeaponStage : HiglightingUIStage
{
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private WeaponCraftPanel _weaponCraftPanel;
    [SerializeField] private Vector3 _offsetForHandClick;
    [SerializeField] private Vector3 _offsetForUiOutline;
    [SerializeField] private ScrollRect _scrollRect;

    private Canvas _handClickCanvas;
    protected override void OnEnter()
    {
        base.OnEnter();
        _handClickCanvas = _handClickView.gameObject.AddComponent<Canvas>();
        StartCoroutine(WaitOneFrame());
    }

    protected override void OnExit()
    {
        base.OnExit();

        Destroy(_handClickCanvas.gameObject);
        _scrollRect.enabled = true;
        _handClickView.transform.SetParent(transform);
        _handClickView.gameObject.SetActive(false);
        _uIOutline.transform.SetParent(transform);
        _uIOutline.gameObject.SetActive(false);
    }

    private IEnumerator WaitOneFrame()
    {
        yield return new WaitForEndOfFrame();

        _scrollRect.enabled = false;
        _handClickView.transform.SetParent(_weaponCraftPanel.CraftItemButtons[0].transform, false);
        _uIOutline.transform.SetParent(_weaponCraftPanel.CraftItemButtons[0].transform, false);
        _uIOutline.transform.SetSiblingIndex(0);
        _uIOutline.GetComponent<RectTransform>().anchoredPosition = _offsetForUiOutline;
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _uIOutline.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        _handClickCanvas.overrideSorting = true;
        _handClickCanvas.sortingOrder = 101;
        _weaponCraftPanel.CraftItemButtons[0].Selected += OnButtonSelected;
    }

    private void OnButtonSelected(CraftItemButton craftItemButton)
    {
        craftItemButton.Selected -= OnButtonSelected;

        Complete();
    }
}
