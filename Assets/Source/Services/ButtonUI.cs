using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonUI : MonoBehaviour
{
    [SerializeField] private Button _button;

    public Button Button => _button;

    public event Action Clicked;

    private void Awake()
    {
        if (_button == null)
            _button = GetComponent<Button>();
    }

    private void OnEnable() => _button.onClick.AddListener(OnButtonClicked);

    private void OnDisable() => _button.onClick.RemoveListener(OnButtonClicked);

    public void Click() => Clicked?.Invoke();

    private void OnButtonClicked() => Clicked?.Invoke();
}
