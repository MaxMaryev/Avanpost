using System.Collections;
using UnityEngine;

public class Notifier : MonoBehaviour
{
    [SerializeField] private TimeDay _timeDay;
    [SerializeField] private GameObject _view;
    [SerializeField] private Animator _animator;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _timeDay.Started += OnTimeDayStarted;
    }

    private void OnDisable()
    {
        _timeDay.Started -= OnTimeDayStarted;
    }

    private void OnTimeDayStarted()
    {
        _view.SetActive(true);
        _coroutine = StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        _view.gameObject.SetActive(false);
    }
}
