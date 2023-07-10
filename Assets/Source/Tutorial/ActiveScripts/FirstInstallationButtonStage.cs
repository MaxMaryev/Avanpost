using UnityEngine;

public class FirstInstallationButtonStage : HiglightingUIStage
{
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Transform _rootForHandClick;
    [SerializeField] private Vector3 _offsetForHandClick;
    [SerializeField] private InstallationButton _installationButton;
    [SerializeField] private Vector3 _offsetUIOutline;

    protected override void OnEnter()
    {
        base.OnEnter();
        _handClickView.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetSiblingIndex(0);
        _uIOutline.GetComponent<RectTransform>().anchoredPosition = _offsetUIOutline;
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _uIOutline.gameObject.SetActive(true);
        _installationButton.Clicked += OnBreakButtonClicked;
    }

    private void OnBreakButtonClicked()
    {
        _installationButton.Clicked -= OnBreakButtonClicked;
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
