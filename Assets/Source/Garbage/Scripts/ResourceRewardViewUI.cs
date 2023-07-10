using UnityEngine;

public class ResourceRewardViewUI :ResourceRewardView
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;

    private float _startAlpha;

    protected override void Awake()
    {
        _startAlpha = _canvasGroup.alpha;
    }

    protected override void Restart()
    {
        _canvasGroup.alpha = 1;
    }

    protected override void Move(float speed) => _rectTransform.anchoredPosition += new Vector2(0, speed * Time.deltaTime);

    protected override void Fade(float currentTime, float timeToDeactivate) => _canvasGroup.alpha = Mathf.Lerp(_startAlpha, 0, currentTime / timeToDeactivate);
}
