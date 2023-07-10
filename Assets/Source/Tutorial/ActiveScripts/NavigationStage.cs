using System;
using UnityEngine;

public class NavigationStage : TutorialStage
{
    [SerializeField] private ArrowIndicates _template;
    [SerializeField] private Transform _target;
    [SerializeField] private MonoBehaviour _navigation;
    [SerializeField] private Vector3 _offsetArrowIndicates;
    [SerializeField] private string _message;

    [field: SerializeField] protected HintHandler HintHandler { get; private set; }

    private ArrowIndicates _arrowIndicates;

    private INavigatable _navigatable => (INavigatable)_navigation;

    public override event Action<TutorialStage> Completed;

    private void OnValidate()
    {
        if (_navigation && !(_navigation is INavigatable))
        {
            Debug.LogError(nameof(_navigation) + " needs to implement " + nameof(INavigatable));
            _navigation = null;
        }
    }

    protected override void OnEnter()
    {
        HintHandler.Add(_message);
        _navigatable.Navigate(_target.position);
        _navigatable.Reached += OnTargetReached;

        _arrowIndicates = Instantiate(_template, _target);
        _arrowIndicates.transform.localPosition = _offsetArrowIndicates;
        _arrowIndicates.Init();
    }

    protected override void OnExit()
    {
        if (_arrowIndicates != null)
            Destroy(_arrowIndicates.gameObject);
    }

    protected float GetDistanceToTarget(Vector3 position)
    {
        return (_target.position - position).SqrMagnitudeXZ();
    }

    private void OnTargetReached()
    {
        _navigatable.Reached -= OnTargetReached;
        Complete();
    }

    protected void Complete() => Completed?.Invoke(this);

}
