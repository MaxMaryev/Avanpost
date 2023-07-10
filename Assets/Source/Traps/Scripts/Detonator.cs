using UnityEngine;

public class Detonator : MonoBehaviour
{
    [SerializeField] private ActivateBombsButton _activateBombsButton;
    [SerializeField] private Transform _activeTrapsContainer;

    private void OnEnable() => _activateBombsButton.Clicked += OnClicked;

    private void OnDisable() => _activateBombsButton.Clicked -= OnClicked;

    private Bomb[] GetBombsList() => _activeTrapsContainer.GetComponentsInChildren<Bomb>();

    private void OnClicked()
    {
        GetBombsList();

        foreach (var bomb in GetBombsList())
            bomb.Explode();
    }
}
