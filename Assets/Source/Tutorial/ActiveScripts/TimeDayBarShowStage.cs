using System.Collections;
using UnityEngine;

public class TimeDayBarShowStage : HiglightingUIStage
{
    [SerializeField] private UIOutline _uIOutline;
    [SerializeField] private HandClickView _handClickView;
    [SerializeField] private Vector3 _offsetForHandClick;
    [SerializeField] private Vector3 _offsetForOutline;
    [SerializeField] private TimeDayView _timeDayView;

    protected override void OnEnter()
    {
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
        _timeDayView.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        base.OnEnter();

        _handClickView.transform.SetParent(_timeDayView.transform, false);
        _uIOutline.transform.SetParent(_timeDayView.transform, false);
        _uIOutline.transform.SetSiblingIndex(0);
        _uIOutline.GetComponent<RectTransform>().anchoredPosition = _offsetForOutline;
        _handClickView.GetComponent<RectTransform>().anchoredPosition = _offsetForHandClick;
        _uIOutline.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Mouse0));
        Complete();
    }
}
