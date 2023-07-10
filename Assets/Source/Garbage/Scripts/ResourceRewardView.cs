using System.Collections;
using TMPro;
using UnityEngine;

public class ResourceRewardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _amountView;
    [SerializeField] private float _speed;

    private Vector3 _startScale;
    private float _timeToDeactivate = 1.5f;
    private float _currentTime = 0;

    protected virtual void Awake()
    {
        _startScale = transform.localScale;
    }

    public void Init(int amount)
    {
        _amountView.text = "+" + amount.ToString();
        Restart();
        _currentTime = 0;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        Move(_speed);
        Fade(_currentTime, _timeToDeactivate);

        if (_currentTime >= _timeToDeactivate)
            gameObject.SetActive(false);
    }

    protected virtual void Restart() => transform.localScale = _startScale;
    protected virtual void Move(float speed) => transform.Translate(transform.up * _speed * Time.deltaTime);
    protected virtual void Fade(float currentTime, float timeToDeactivate) => transform.localScale = Vector3.Lerp(_startScale, Vector3.zero, currentTime / timeToDeactivate);
}
