using System;
using System.Collections;
using UnityEngine;

public class ShowParametersStage : HiglightingUIStage
{
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Transform _rootForHandClick;
    [SerializeField] private Vector3 _offsetForHandClick;
    [SerializeField] private Vector3 _offsetOutline;

    protected override void OnEnter()
    {
        base.OnEnter();

        _handClickView.transform.SetParent(_rootForHandClick, false);
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _uIOutline.transform.SetParent(_rootForHandClick, false);
        _uIOutline.GetComponent<RectTransform>().anchoredPosition = _offsetOutline;
        _uIOutline.gameObject.SetActive(true);
        StartCoroutine(WaitTap());
    }

    protected override void OnExit()
    {
        base.OnExit();

        _handClickView.transform.SetParent(transform);
        _handClickView.gameObject.SetActive(false);
        _uIOutline.transform.SetParent(transform);
        _uIOutline.gameObject.SetActive(false);
    }

    private IEnumerator WaitTap()
    {
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));

        Complete();
    }
}
