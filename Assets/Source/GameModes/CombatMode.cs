using UnityEngine;

public class CombatMode : IGameMode
{
    private IShooter _player;
    private WeaponPanel _weaponPanel;
    private Canvas _rewardWeaponsCanvas;
    private Canvas _gameMenu;

    public CombatMode(IShooter player, WeaponPanel weaponPanel, Canvas rewardWeaponsCanvas, Canvas gameMenu)
    {
        _player = player;
        _weaponPanel = weaponPanel;
        _rewardWeaponsCanvas = rewardWeaponsCanvas;
        _gameMenu = gameMenu;
    }

    public void SetActive(bool state)
    {
        _weaponPanel.gameObject.SetActive(state);
        _rewardWeaponsCanvas.enabled = state;
        _gameMenu.enabled = state;

        if (state)
            _player.BeginTryShooting();
        else
            _player.StopShooting();
    }
}
