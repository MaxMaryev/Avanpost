using System.Collections;
using UnityEngine;

public class ShowWalletStage : HiglightingUIStage
{
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Transform _rootForHandClick;
    [SerializeField] private Vector3 _offsetForHandClick;
    [SerializeField] private GameObject _walletView;

    protected override void OnEnter()
    {
        StartCoroutine(WaitClick());
    }

    protected override void OnExit()
    {
        base.OnExit();
        _handClickView.transform.SetParent(transform);
        _handClickView.gameObject.SetActive(false);
        _uIOutline.transform.SetParent(transform);
        _uIOutline.gameObject.SetActive(false);
    }

    private IEnumerator WaitClick()
    {
        _walletView.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        base.OnEnter();
        _handClickView.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetParent(_rootForHandClick, false);
        _uIOutline.transform.SetSiblingIndex(0);
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _uIOutline.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Mouse0));

        Complete();
    }
}
