using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Agava.YandexGames;

public class BuildingsPanel : MonoBehaviour
{
    [SerializeField] private BuildingSlotsDatasSO _buildingSlotsDatasSO;
    [SerializeField] private BuildingSlotButton _buidingSlotButtonTemplate;
    [SerializeField] private List<SubmenuButton> _submenuButtons;
    [SerializeField] private ObstaclesSubmenu _obstaclesSubmenu;
    [SerializeField] private TurretsSubmenu _turretsSubmenu;
    [SerializeField] private TrapsSubmenu _trapsSubmenu;
    [SerializeField] private LightsSubmenu _lightsSubmenu;
    [SerializeField] private ExitButton _exitButton;
    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private WalletPresenter _walletPresenter;

    [SerializeField] private TimeScaleHandler _timeScaleHandler;
    [SerializeField] private DayCycleManager _dayCycleManager;

    private List<BuildingSlotButton> _slotButtons = new List<BuildingSlotButton>();

    public event Action<BuildingSlotButton> Added;
    public event Action Exited;
    public event Action Opened;

    public IReadOnlyList<BuildingSlotButton> SlotButtons => _slotButtons;
    public bool SlotsCreated { get; private set; }

    private void Awake()
    {
        IReadOnlyList<BuildingSlotData> buildingSlotsDatas = _buildingSlotsDatasSO.SlotsDatas;

        InstantiateSlots(buildingSlotsDatas, BuildingClass.Obstacle, _obstaclesSubmenu.transform);
        InstantiateSlots(buildingSlotsDatas, BuildingClass.Turret, _turretsSubmenu.transform);
        InstantiateSlots(buildingSlotsDatas, BuildingClass.Trap, _trapsSubmenu.transform);
        InstantiateSlots(buildingSlotsDatas, BuildingClass.Light, _lightsSubmenu.transform);
        SlotsCreated = true;

        OpenSubmenu();
    }

    private void OnEnable()
    {
        Opened?.Invoke();

        foreach (var submenuButton in _submenuButtons)
            submenuButton.Clicked += OnSubmenuButtonClicked;

        _exitButton.Clicked += OnExitButtonClicked;

#if UNITY_WEBGL && !UNITY_EDITOR
        if(YandexGamesSdk.IsInitialized)
            if(Agava.YandexGames.Device.Type == Agava.YandexGames.DeviceType.Mobile)
                Agava.YandexGames.StickyAd.Hide();
#endif
    }

    private void OnDisable()
    {
        foreach (var submenuButton in _submenuButtons)
            submenuButton.Clicked -= OnSubmenuButtonClicked;

        _exitButton.Clicked -= OnExitButtonClicked;
    }

    private void OnSubmenuButtonClicked(SubmenuButton submenuButton)
    {
        _obstaclesSubmenu.gameObject.SetActive(false);
        _turretsSubmenu.gameObject.SetActive(false);
        _trapsSubmenu.gameObject.SetActive(false);
        _lightsSubmenu.gameObject.SetActive(false);

        if (submenuButton is ObstaclesSubmenuButton)
            _obstaclesSubmenu.gameObject.SetActive(true);
        else if (submenuButton is TurretsSubmenuButton)
            _turretsSubmenu.gameObject.SetActive(true);
        else if (submenuButton is TrapsSubmenuButton)
            _trapsSubmenu.gameObject.SetActive(true);
        else if (submenuButton is LightsSubmenuButton)
            _lightsSubmenu.gameObject.SetActive(true);
    }

    private void InstantiateSlots(IReadOnlyList<BuildingSlotData> buildingSlotsDatas, BuildingClass buildingClass, Transform submenuTransform)
    {
        IReadOnlyList<BuildingSlotData> slotsDatas = buildingSlotsDatas.Where(x => x.Class == buildingClass).ToList();

        for (int i = 0; i < slotsDatas.Count; i++)
        {
            BuildingSlotButton buildingSlotButton = Instantiate(_buidingSlotButtonTemplate, submenuTransform);
            buildingSlotButton.Init(slotsDatas[i], _toggleGroup, _walletPresenter);
            _slotButtons.Add(buildingSlotButton);
            Added?.Invoke(buildingSlotButton);
        }
    }

    private void OnExitButtonClicked()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (_dayCycleManager.CurrentDay > 2)
        {
            Agava.YandexGames.InterstitialAd.Show(() => _timeScaleHandler.FreezeTime(), onCloseCallback: (isClosed) =>
            {
                _timeScaleHandler.UnFreezeTime();
            });
        }

        Agava.YandexGames.StickyAd.Show();
#endif
        Exited?.Invoke();
    }

    private void OpenSubmenu() => _submenuButtons.FirstOrDefault(x => x.Toggle.isOn = true).Submenu.gameObject.SetActive(true);
}
