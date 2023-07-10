using UnityEngine;
using UnityEngine.UI;

public class VolumeMainSettings : MonoBehaviour
{
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Sprite _soundSprite100;
    [SerializeField] private Sprite _soundSprite50;
    [SerializeField] private Sprite _soundSprite0;
    [SerializeField] private Image _icon;

    private void OnEnable()
    {
        _soundSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDisable()
    {
        _soundSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void Start()
    {
        _soundSlider.value = AudioListener.volume;
        OnSliderValueChanged(_soundSlider.value);
    }

    private void OnSliderValueChanged(float value)
    {
        if (value > 0.6f)
            _icon.sprite = _soundSprite100;
        else if (value > 0)
            _icon.sprite = _soundSprite50;
        else
            _icon.sprite = _soundSprite0;

        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }
}
