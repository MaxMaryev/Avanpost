using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GarbagePointHandler : MonoBehaviour, IFriskableHandler
{
    [SerializeField] private PlayerAnimationsController _playerAnimationsController;
    [SerializeField] private List<GarbagePoint> _garbagePoints = new List<GarbagePoint>();
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private int _maxCountSpawnGarbage = 3;
    [SerializeField] private int _baseValueReward;
    [SerializeField] private int _rewardMultiplier;
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private int _baseValueActivatePoints = 6;
    [SerializeField] private int _maxValueActivatePoints = 11;

    private GarbageSaver _garbageSaver;

    public int NumbersOfAvailableFriskable => Mathf.Clamp(_baseValueActivatePoints + _dayCycleManager.CurrentDay, _baseValueActivatePoints, _maxValueActivatePoints);
    public IReadOnlyList<IFriskable> Friskables => _garbagePoints;

    private void Awake()
    {
        _garbageSaver = new GarbageSaver();
        LoadGarbagePoints();
    }

    private void OnEnable()
    {
        _dayCycleManager.TimeDayChanged += OnTimeDayChanged;
        OnTimeDayChanged(_dayCycleManager.CurrentTimeDay);
    }

    private void OnDisable()
    {
        _dayCycleManager.TimeDayChanged -= OnTimeDayChanged;

        foreach (var garbagePoint in _garbagePoints)
            garbagePoint.Frisked -= OnGarbageFrisked;
    }

    private void OnTimeDayChanged(TimeDay timeDay)
    {
        if (timeDay is Dawn)
            ActivateGarbagePoints();
        else if (timeDay is Night)
            DeactivateGarbagePoints();
    }

    private void LoadGarbagePoints()
    {
        for (int i = 0; i < _garbagePoints.Count; i++)
        {
            bool isActive = _garbageSaver.Load(i) == 1;
            _garbagePoints[i].gameObject.SetActive(isActive);

            if (isActive)
            {
                _garbagePoints[i].Init(UnityEngine.Random.Range(1, _maxCountSpawnGarbage + 1), _playerAnimationsController);
                _garbagePoints[i].Frisked += OnGarbageFrisked;
            }
        }
    }

    private void SaveGarbagePoint()
    {
        for (int i = 0; i < _garbagePoints.Count; i++)
            _garbageSaver.Save(_garbagePoints[i].gameObject.activeSelf ? 1 : 0, i);
    }

    private void ActivateGarbagePoints()
    {
        if (_garbagePoints.Where(x => x.gameObject.activeSelf).FirstOrDefault() != null)
            return;

        List<GarbagePoint> allGarbagePoints = new List<GarbagePoint>(); 
        List<GarbagePoint> resultGarbagePointsToActivate = new List<GarbagePoint>();
        allGarbagePoints.AddRange(_garbagePoints.ToArray());

        for (int i = 0; i <= NumbersOfAvailableFriskable - 1; i++)
        {
            int rand = UnityEngine.Random.Range(0, allGarbagePoints.Count);
            resultGarbagePointsToActivate.Add(allGarbagePoints[i]);
            allGarbagePoints.Remove(allGarbagePoints[i]);
        }

        foreach (var garbagePoint in resultGarbagePointsToActivate)
            ActivateGarbagePoint(garbagePoint);

        SaveGarbagePoint();
    }

    public void ActivateGarbagePoint(GarbagePoint garbagePoint)
    {
        if (garbagePoint.gameObject.activeSelf)
            return;

        garbagePoint.gameObject.SetActive(true);
        garbagePoint.Init(UnityEngine.Random.Range(1, _maxCountSpawnGarbage + 1), _playerAnimationsController);
        garbagePoint.Frisked += OnGarbageFrisked;
    }

    private void DeactivateGarbagePoints()
    {
        foreach (var garbagePoint in _garbagePoints)
        {
            if (garbagePoint.gameObject.activeSelf)
            {
                garbagePoint.Frisked -= OnGarbageFrisked;
                garbagePoint.gameObject.SetActive(false);
            }
        }

        SaveGarbagePoint();
    }

    private void OnGarbageFrisked(IFriskable garbagePoint)
    {
        garbagePoint.Frisked -= OnGarbageFrisked;
        garbagePoint.GameObject.SetActive(false);

        if (garbagePoint.IsFilled)
            for (int i = 0; i < garbagePoint.NumberOfElements; i++)
                _resourceSpawner.SpawnResource(garbagePoint.Transform.position, garbagePoint.GetTargetPositionForSpawnElement(), CalculateRewardForGarbage());

        SaveGarbagePoint();
    }

    private int CalculateRewardForGarbage()
    {
        return (_baseValueReward + (_rewardMultiplier * _dayCycleManager.CurrentDay));
    }
}
