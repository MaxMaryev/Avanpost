using System;
using System.Collections;
using UnityEngine;

public class SelectGateStage : HiglightingUIStage
{
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private ObstaclesSubmenu _obstaclesSubmenu;
    [SerializeField] private Vector3 _offsetForHandClick;

    private BuildingSlotButton _buildingSlotButton;

    protected override void OnEnter()
    {
        base.OnEnter();

        foreach (var slot in _obstaclesSubmenu.BuildingSlotButtons)
            if (slot.SlotData.Name == BuildingName.Gate_Lvl_1)
                _buildingSlotButton = slot;

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

        _handClickView.transform.SetParent(_buildingSlotButton.transform, false);
        _uIOutline.transform.SetParent(_buildingSlotButton.transform, false);
        _uIOutline.transform.SetSiblingIndex(0);
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _uIOutline.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        _buildingSlotButton.Selected += OnButtonSelected;
    }

    private void OnButtonSelected(BuildingSlotData buildingSlotData)
    {
        _buildingSlotButton.Selected -= OnButtonSelected;
        _buildingSlotButton.SetToggle(true);

        Complete();
    }
}
