using System.Collections;
using UnityEngine;

public class ArrowIndicates : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxOffsetY;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _currentTime;

    public void Init()
    {
        _startPosition = transform.localPosition;
        _endPosition = new Vector3(_startPosition.x, _startPosition.y - _maxOffsetY, _startPosition.z);
        _currentTime = 0;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime * _speed;
        transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, Mathf.PingPong(_currentTime, 1));
        transform.rotation *= Quaternion.Euler(0, _speed * Time.unscaledDeltaTime, 0);
    }
}
