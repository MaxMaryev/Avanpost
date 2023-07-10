using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SearchFriskableInCityStage : TutorialStage
{
    [SerializeField] private Player _player;
    [SerializeField] private ArrowIndicates _templateArrow;
    [SerializeField] private MonoBehaviour _navigation;
    [SerializeField] private TaskBarHandler _taskBarHandler;
    [SerializeField] private MonoBehaviour _friskHandler;
    [SerializeField] private Task _task;

    private Coroutine _coroutine;

    private ITaskActivable _taskActivable => _task;
    private IFriskableHandler _friskableHandler => (IFriskableHandler)_friskHandler;
    private INavigatable _navigatable => (INavigatable)_navigation;
    private List<IFriskable> _activeGarbagePoints = new List<IFriskable>();
    private Dictionary<IFriskable, ArrowIndicates> _arrowIndicatesForGarbagePoints = new Dictionary<IFriskable, ArrowIndicates>();

    public override event Action<TutorialStage> Completed;

    private void OnValidate()
    {
        if (_navigation && !(_navigation is INavigatable))
        {
            Debug.LogError(nameof(_navigation) + " needs to implement " + nameof(INavigatable));
            _navigation = null;
        }

        if (_friskHandler && !(_friskHandler is IFriskableHandler))
        {
            Debug.LogError(nameof(_friskHandler) + " needs to implement " + nameof(IFriskableHandler));
            _friskHandler = null;
        }
    }

    protected override void OnEnter()
    {
        _activeGarbagePoints.AddRange(_friskableHandler.Friskables.Where(x => x.GameObject.activeSelf == true));

        if (_activeGarbagePoints.Count == 0)
        {
            Completed?.Invoke(this);
            return;
        }


        foreach (var point in _activeGarbagePoints)
        {
            ArrowIndicates arrowIndicates = Instantiate(_templateArrow, point.Transform);
            arrowIndicates.transform.localPosition = new Vector3(0, 3, 0);
            arrowIndicates.Init();
            _arrowIndicatesForGarbagePoints.Add(point, arrowIndicates);
            point.Frisked += OnGarbagePointFrisked;
        }

        _taskActivable.TryActivate();
        _taskActivable.Finished += OnTaskableFinished;
        _taskBarHandler.AddTask(_taskActivable);
        _coroutine = StartCoroutine(SearchNearestGarbagePoint());
    }

    private void OnTaskableFinished(ITaskable taskable)
    {
        taskable.Finished -= OnTaskableFinished;
        Completed?.Invoke(this);
    }

    private void OnGarbagePointFrisked(IFriskable garbagePoint)
    {
        garbagePoint.Frisked -= OnGarbagePointFrisked;
        Destroy(_arrowIndicatesForGarbagePoints[garbagePoint].gameObject);
        _arrowIndicatesForGarbagePoints.Remove(garbagePoint);
        _activeGarbagePoints.Remove(garbagePoint);
    }

    protected override void OnExit()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator SearchNearestGarbagePoint()
    {
        Vector3 lastTargetForNavigate = Vector3.zero;

        while (_activeGarbagePoints.Count > 0)
        {
            IFriskable garbagePoint = GetNearestGarbagePoint();

            if (lastTargetForNavigate != garbagePoint.Transform.position)
            {
                _navigatable.Navigate(garbagePoint.Transform.position);
                lastTargetForNavigate = garbagePoint.Transform.position;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IFriskable GetNearestGarbagePoint()
    {
        IFriskable nearestGarbagePoint = _activeGarbagePoints[0];
        float lastDistance = (_player.transform.position - nearestGarbagePoint.Transform.position).SqrMagnitudeXZ();

        foreach (var point in _activeGarbagePoints)
        {
            float _distanceToPoint = (_player.transform.position - point.Transform.position).SqrMagnitudeXZ();

            if (_distanceToPoint < lastDistance)
            {
                lastDistance = _distanceToPoint;
                nearestGarbagePoint = point;
            }
        }

        return nearestGarbagePoint;
    }
}
