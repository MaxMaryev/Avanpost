using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTakingDamageEffect : MonoBehaviour
{
    [SerializeField] private List<Sprite> _bloodEffects = new List<Sprite>();
    [SerializeField] private Player _player;
    [SerializeField] private Image _image;

    private void OnDisable()
    {
        _player.Health.ValueChanged -= OnHealthChanged;
    }

    private void Start()
    {
        _player.Health.ValueChanged += OnHealthChanged;
        OnHealthChanged(_player.Health.MaxValue, _player.Health.CurrentValue);
    }

    private void OnHealthChanged(float maxValue, float currentValue)
    {
        int value = Mathf.Clamp((int)(currentValue / maxValue * 10), 0, _bloodEffects.Count - 1);

        _image.enabled = !(currentValue >= maxValue);
        _image.sprite = _bloodEffects[value];

        if (_player.IsAlive == false)
            gameObject.SetActive(false);
    }
}
