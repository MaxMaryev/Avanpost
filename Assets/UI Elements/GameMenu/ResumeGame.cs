using UnityEngine;

public class ResumeGame : MonoBehaviour
{
    [SerializeField] private GameMenu _gameMenu;
    [SerializeField] private ResumeButton _resumeButton;

    private void OnEnable() => _resumeButton.Clicked += OnResumeButtonClicked;

    private void OnDisable() => _resumeButton.Clicked -= OnResumeButtonClicked;

    public void OnResumeButtonClicked()
    {
        _gameMenu.ShowButtons(false);
    }
}
