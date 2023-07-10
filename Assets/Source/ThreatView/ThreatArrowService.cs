using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreatArrowService : MonoBehaviour
{
    [SerializeField] private int _maxArrows = 10;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private Transform _player;
    [SerializeField] private ThreatArrow _template;
    [SerializeField] private Transform _obstaclesContaner;
    [SerializeField] private BuildingInstallation _buildingInstallation;
    [SerializeField] private List<Sprite> _icons;
    [SerializeField] private BuildingsSaver _buildingsSaver;
    [SerializeField] private CanvasScaler _canvasScaler;

    private List<ThreatArrow> _threatArrows = new List<ThreatArrow>();
    private List<Building> _obstacles = new List<Building>();

    private void Start() => StartCoroutine(DelayedInit());

    private void OnDisable()
    {
        _buildingInstallation.Done -= OnBuildingBuilt;

        foreach (var obstacle in _obstacles)
            obstacle.SafetyMarginChanged -= OnSafetyMarginChanged;
    }

    private void OnSafetyMarginChanged(float maxValue, float currentValue, Building building)
    {
        if (_threatArrows.Count >= _maxArrows)
            return;

        if (currentValue / maxValue >= 0.33f)
            return;

        if (currentValue <= 0)
        {
            building.SafetyMarginChanged -= OnSafetyMarginChanged;
            _obstacles.Remove(building);
            return;
        }

        for (int i = 0; i < _threatArrows.Count; i++)
        {
            if (_threatArrows[i].Building == building)
            {
                _threatArrows[i].RebootTimer();
                return;
            }
        }

        var threatArrow = Instantiate(_template, _canvas);
        threatArrow.Init(_player, building, _canvasScaler, _icons[Random.Range(0, _icons.Count)]);
        _threatArrows.Add(threatArrow);
    }

    private void OnBuildingBuilt(Building building)
    {
        if (building is Fence || building is Gate)
            _obstacles.Add(building);

        building.SafetyMarginChanged += OnSafetyMarginChanged;
    }

    private IEnumerator DelayedInit()
    {
        yield return new WaitUntil(() => _buildingsSaver.HasLoaded);

        _buildingInstallation.Done += OnBuildingBuilt;
        _obstacles.AddRange(_obstaclesContaner.GetComponentsInChildren<Building>());

        foreach (var obstacle in _obstacles)
            obstacle.SafetyMarginChanged += OnSafetyMarginChanged;
    }
}
