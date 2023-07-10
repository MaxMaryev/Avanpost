using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ConstructorMode : IGameMode
{
    private CinemachineVirtualCamera _houseCamera;
    private AllCanvases _allCanvases;
    private NavMeshAgent _playerAgent;
    private ConstructorGrid _constructorGrid;
    private BuildingsPanel _buildingsPanel;

    private BreakButton _breakButton;
    private InstallationButton _installationButton;
    private BuildingInstallation _buildingInstallation;
    private BuildingRemoving _buildingRemoving;

    public ConstructorMode(CinemachineVirtualCamera houseCamera, AllCanvases allCanvases, NavMeshAgent playerAgent,
                           ConstructorGrid constructorGrid, BuildingsPanel buildingsPanel, BreakButton breakButton,
                           BuildingInstallation buildingInstallation, BuildingRemoving buildingRemoving, InstallationButton installationButton)
    {
        _houseCamera = houseCamera;
        _allCanvases = allCanvases;
        _playerAgent = playerAgent;
        _constructorGrid = constructorGrid;
        _buildingsPanel = buildingsPanel;
        _breakButton = breakButton;
        _buildingInstallation = buildingInstallation;
        _buildingRemoving = buildingRemoving;
        _installationButton = installationButton;
    }

    public void SetActive(bool state)
    {
        if (state)
        {
            _breakButton.Clicked += OnBreakButtonClicked;
            _installationButton.Clicked += OnInstallationButtonClicked;
        }
        else
        {
            _breakButton.Clicked -= OnBreakButtonClicked;
            _installationButton.Clicked -= OnInstallationButtonClicked;
        }

        _constructorGrid.gameObject.SetActive(state);
        _buildingsPanel.gameObject.SetActive(state);
        _playerAgent.enabled = !state;
        _allCanvases.gameObject.SetActive(!state);

        _houseCamera.enabled = state;
    }


    private void OnBreakButtonClicked()
    {
        _buildingInstallation.enabled = false;
        _buildingRemoving.enabled = true;
    }

    private void OnInstallationButtonClicked()
    {
        _buildingInstallation.enabled = true;
        _buildingRemoving.enabled = false;
    }
}
