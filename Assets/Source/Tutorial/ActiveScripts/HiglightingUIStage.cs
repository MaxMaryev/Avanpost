using System;
using UnityEngine;
using UnityEngine.UI;

public class HiglightingUIStage : TutorialStage
{
    [SerializeField] private Canvas _bacgroundBlack;
    [SerializeField] private GameObject _selectedObject;

    private Canvas _canvas;
    private GraphicRaycaster _graphicRaycaster;

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        _bacgroundBlack.gameObject.SetActive(true);
        _canvas = _selectedObject.AddComponent<Canvas>();
        _canvas.overrideSorting = true;
        _canvas.sortingOrder = 100;
        _graphicRaycaster = _selectedObject.AddComponent<GraphicRaycaster>();
    }

    protected override void OnExit()
    {
        Destroy(_graphicRaycaster);
        Destroy(_canvas);
        _bacgroundBlack.gameObject.SetActive(false);
    }

    protected void Complete() => Completed?.Invoke(this);
}
