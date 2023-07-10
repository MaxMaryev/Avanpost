using UnityEngine;
using System;
using UnityEngine.UI;

public class GateLock : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private Sprite _lockSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _unlockSprite;
    [SerializeField] private Transform _gateTransform;
    [SerializeField] private float _height;
    [SerializeField] private float _offset;
    [SerializeField] private Color _lockColor;
    [SerializeField] private Color _unlockColor;

    private bool _isOpen;
    private Camera _camera;
    private Vector3 _cameraDirection;

    public event Action<BoxCollider> Opened;
    public event Action<BoxCollider> Closed;

    private void Awake()
    {
        _camera = Camera.main;
        _spriteRenderer.sprite = _lockSprite;
        //_spriteRenderer.color = _lockColor;
    }

    private void Update()
    {
        _cameraDirection = (_camera.transform.position - _gateTransform.position).With(y: _height).normalized;
        transform.position = _gateTransform.position.With(y: _height) + _cameraDirection * _offset;
        transform.forward = _camera.transform.forward;
    }

    private void OnMouseDown()
    {
        if (_isOpen)
        {
            Close();
            _spriteRenderer.sprite = _lockSprite;
            //_spriteRenderer.color = _lockColor;
            _isOpen = false;
        }
        else
        {
            Open();
            _spriteRenderer.sprite = _unlockSprite;
            //_spriteRenderer.color = _unlockColor;
            _isOpen = true;
        }
    }

    private void Open() => Opened?.Invoke(_collider);

    private void Close() => Closed?.Invoke(_collider);
}
