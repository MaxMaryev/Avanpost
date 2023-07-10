using UnityEngine;

public class ButtonClickSound : PlayClickSound
{
    [SerializeField] private ButtonUI _button;

    private void OnEnable()
    {
        _button.Clicked += OnButtonClicked;
    }

    private void OnDisable()
    {
        _button.Clicked -= OnButtonClicked;
    }

    private void OnButtonClicked() => PlaySound();
}
