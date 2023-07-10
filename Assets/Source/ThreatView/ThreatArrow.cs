using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThreatArrow : MonoBehaviour
{
    [SerializeField] private Image _image;

    private Transform _player;
    private Camera _camera;
    private RectTransform _selfRect;
    private Vector2 _screenCenter;
    private Vector2 _position;
    private Vector2 _halfRect;
    private float _time;
    private float _maxDuration;
    private Transform _obstacleTransform;
    private CanvasScaler _canvasScaler;
    private float _height;
    private float _width;

    public Building Building { get; private set; }

    private void Awake()
    {
        _selfRect = transform as RectTransform;

        _camera = Camera.main;
        _halfRect = _selfRect.sizeDelta / 2f;
        _maxDuration = 3;
        StartCoroutine(Blinking());
    }

    public void Init(Transform player, Building building, CanvasScaler canvasScaler, Sprite sprite)
    {
        _player = player;
        _canvasScaler = canvasScaler;
        _height = (1080 - _canvasScaler.referenceResolution.x * Screen.height / Screen.width) / 2 + _canvasScaler.referenceResolution.x * Screen.height / Screen.width;
        _width = 1920;

        _screenCenter = new Vector2(_width / 2f, _height / 2f);
        Building = building;
        _image.sprite = sprite;

        if (building is Fence fence)
            _obstacleTransform = fence.CurrentState.transform;
        else
            _obstacleTransform = building.transform;
    }

    public void RebootTimer() => _time = 0;

    private void LateUpdate()
    {
        _time += Time.deltaTime;

        if (Building == null)
            return;

        if (Building.CurrentSafetyMargin <= 0 || _time > _maxDuration)
        {
            Destroy(gameObject);
            return;
        }

        var direction = (_obstacleTransform.position - _player.transform.position).normalized;
        _position = _screenCenter + new Vector2(direction.x, direction.z) * _width / 2f;

        var angle = Vector2.SignedAngle(Vector2.left, _position - _screenCenter);
        _selfRect.rotation = Quaternion.Euler(0, 0, angle) * Quaternion.Euler(0, 0, 90);

        _position.x = Mathf.Clamp(_position.x, _halfRect.x, _width - _halfRect.x);
        _position.y = Mathf.Clamp(_position.y, _halfRect.y, _height - _halfRect.y);

        _selfRect.anchoredPosition = _position;
    }

    private IEnumerator Blinking()
    {
        Color defaultColor = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
        Color fade = new Color(_image.color.r, _image.color.g, _image.color.b, 0);

        bool _isFading;

        yield return new WaitUntil(() => Building != null);

        while (_time < _maxDuration)
        {
            float safetyMarginShare = Building.CurrentSafetyMargin / Building.MaxSafetyMargin;
            float normilizedfloatLerp = safetyMarginShare / 0.33f;

            _image.color = Color.Lerp(_image.color, Color.red, 1 - normilizedfloatLerp);
            yield return null;

            _image.color = fade;
            yield return new WaitForSeconds(safetyMarginShare);

            _image.color = defaultColor;
            yield return new WaitForSeconds(safetyMarginShare);
        }
    }
}
