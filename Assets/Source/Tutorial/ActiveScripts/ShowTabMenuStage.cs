using UnityEngine;

public class ShowTabMenuStage : HiglightingUIStage
{
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private ObstaclesSubmenuButton _obstaclesSubmenuButton;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Transform _rootForHandClick;
    [SerializeField] private Vector3 _offsetForHandClick;

    protected override void OnEnter()
    {
        base.OnEnter();
        _handClickView.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetSiblingIndex(0);
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _uIOutline.gameObject.SetActive(true);
        _obstaclesSubmenuButton.Clicked += OnButtonClicked;
    }

    private void OnButtonClicked(SubmenuButton submenuButton)
    {
        _obstaclesSubmenuButton.Clicked -= OnButtonClicked;
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
