using UnityEngine;
using Cinemachine;
using UnityEngine.AI;
using System;
using Agava.YandexMetrica;

public class GameModeHandler : MonoBehaviour
{
    [SerializeField] private PlayerPresenter _playerPresenter;
    [SerializeField] private Workbench _builderWorkbench;
    [SerializeField] private Workbench _craftWorkbench;
    [SerializeField] private CinemachineVirtualCamera _houseCamera;
    [SerializeField] private AllCanvases _allCanvases;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private WeaponPanel _weaponPanel;
    [SerializeField] private Canvas _rewardWeaponsCanvas;
    [SerializeField] private Canvas _gameMenuCanvas;
    [SerializeField] private ConstructorGrid _constructorGrid;
    [SerializeField] private BuildingsPanel _buildingsPanel;
    [SerializeField] private BreakButton _breakButton;
    [SerializeField] private InstallationButton _installationButton;
    [SerializeField] private BuildingInstallation _buildingInstallation;
    [SerializeField] private BuildingRemoving _buildingRemoving;
    [SerializeField] private CraftMenuPresenter _craftMenuPresenter;

    private IGameMode _combatMode;
    private IGameMode _constructorMode;
    private IGameMode _craftMode;
    private IShooter _player;

    public event Action<GameModes> Changed;

    public GameModes CurrentGameMode { get; private set; }

    private void Awake()
    {
        _player = _playerPresenter.GetComponent<Player>();
        _craftMode = new CraftMode(_allCanvases);
        _combatMode = new CombatMode(_player, _weaponPanel, _rewardWeaponsCanvas, _gameMenuCanvas);
        _constructorMode = new ConstructorMode(_houseCamera, _allCanvases, _navMeshAgent, _constructorGrid, _buildingsPanel,
            _breakButton, _buildingInstallation, _buildingRemoving, _installationButton);
    }

    private void Start() => SwitchTo(GameModes.CombatMode);

    private void OnEnable()
    {
        _builderWorkbench.OpenedCraftMenu += OnOpenedBuilderPanel;
        _builderWorkbench.CloseCraftMenu += OnClosedBuilderPanel;
        _buildingsPanel.Exited += OnClosedBuilderPanel;
        _craftWorkbench.OpenedCraftMenu += OnOpenedCraftMenu;
        _craftWorkbench.CloseCraftMenu += OnClosedCraftMenu;
        _craftMenuPresenter.Exited += OnClosedCraftMenu;
    }

    private void OnDisable()
    {
        _builderWorkbench.OpenedCraftMenu -= OnOpenedBuilderPanel;
        _builderWorkbench.CloseCraftMenu -= OnClosedBuilderPanel;
        _buildingsPanel.Exited -= OnClosedBuilderPanel;
        _craftWorkbench.OpenedCraftMenu -= OnOpenedCraftMenu;
        _craftWorkbench.CloseCraftMenu -= OnClosedCraftMenu;
        _craftMenuPresenter.Exited -= OnClosedCraftMenu;
    }

    private void OnOpenedCraftMenu() => SwitchTo(GameModes.CraftMode);

    private void OnClosedCraftMenu() => SwitchTo(GameModes.CombatMode);

    private void OnOpenedBuilderPanel() => SwitchTo(GameModes.ConstructorMode);

    private void OnClosedBuilderPanel() => SwitchTo(GameModes.CombatMode);

    private void SwitchTo(GameModes gameMode)
    {
        if (gameMode == GameModes.ConstructorMode)
        {
            _combatMode.SetActive(false);
            _craftMode.SetActive(false);
            _constructorMode.SetActive(true);
        }
        else if (gameMode == GameModes.CombatMode)
        {
            _constructorMode.SetActive(false);
            _craftMode.SetActive(false);
            _combatMode.SetActive(true);
        }
        else if (gameMode == GameModes.CraftMode)
        {
            _combatMode.SetActive(false);
            _constructorMode.SetActive(false);
            _craftMode.SetActive(true);
        }

        CurrentGameMode = gameMode;
        Changed?.Invoke(CurrentGameMode);
    }
}

public enum GameModes
{
    ConstructorMode,
    CombatMode,
    CraftMode
}