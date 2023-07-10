using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class RocketLauncher : FiringWeapon
{
    [SerializeField] private float _minShotDistance;
    [SerializeField] private OnTriggerHandler _reloadTrigger;
    [SerializeField] private Building _building;
    [SerializeField] private Rocket _rocket;
    [SerializeField] private Transform _rocketStartPoint;
    [SerializeField] private Transform _point2;
    [SerializeField] private Transform _point3;

    private ObjectsPool[] _zombiePools;
    private Coroutine _waitTargetCoroutine;
    private Coroutine _fireCoroutine;
    private SearchTarget _searchTarget;
    private bool _isPlayerNear;
    private ITargetable _lastTarget;

    public override event Action<float, float> BulletChanged;
    public override event Action<FiringWeapon> Reloaded;

    private void OnEnable()
    {
        _reloadTrigger.TriggerEntered += OnReloadTriggerEntered;
        _reloadTrigger.TriggerExited += OnReloadTriggerExited;
    }

    private void OnDisable()
    {
        _reloadTrigger.TriggerEntered -= OnReloadTriggerEntered;
        _reloadTrigger.TriggerExited -= OnReloadTriggerExited;
    }

    private void Start()
    {
        _waitTargetCoroutine = StartCoroutine(WaitingTarget());
    }

    public void InitZombiesPool(params ObjectsPool[] zombiePools) => _zombiePools = zombiePools;

    private IEnumerator Firing()
    {
        while (CurrentBulletsCount > 0)
        {
            Rocket rocket = Instantiate(_rocket, _rocketStartPoint.position, Quaternion.identity);
            rocket.Init(_point2.position, _point3.position, _lastTarget.Position);

            //ShotEffect.Play();

            CurrentBulletsCount--;
            BulletChanged?.Invoke(ClipCapacity, CurrentBulletsCount);
            yield return new WaitForSeconds(AttackCooldown);
        }

        _fireCoroutine = null;
    }

    private IEnumerator Reload()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(DefaultReloadDuration / ClipCapacity);

        while (_isPlayerNear && CurrentBulletsCount < ClipCapacity)
        {
            yield return waitForSeconds;

            if (_isPlayerNear == false)
                yield break;

            CurrentBulletsCount++;
            BulletChanged?.Invoke(ClipCapacity, CurrentBulletsCount);

            if (CurrentBulletsCount == ClipCapacity)
                Reloaded?.Invoke(this);
        }

        ReloadAnimationsHandler.StopAnimationFrisk();

        if (_waitTargetCoroutine == null)
            _waitTargetCoroutine = StartCoroutine(WaitingTarget());
    }

    private void OnReloadTriggerEntered(Collider other)
    {
        if (_building.IsDestroying)
            return;

        if (CurrentBulletsCount == ClipCapacity)
            return;

        if (other.TryGetComponent(out Player player))
        {
            ReloadAnimationsHandler.PlayAnimationFrisk();
            _isPlayerNear = true;
            StartCoroutine(Reload());

            if (_waitTargetCoroutine == null)
                _waitTargetCoroutine = StartCoroutine(WaitingTarget());
        }
    }

    private void OnReloadTriggerExited(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            ReloadAnimationsHandler.StopAnimationFrisk();
            _isPlayerNear = false;
        }
    }

    private IEnumerator WaitingTarget()
    {
        float updateDelay = 0.3f;
        WaitForSeconds waitUpdateDelay = new WaitForSeconds(updateDelay);
        yield return new WaitUntil(() => _zombiePools != null);
        _searchTarget = new SearchTarget(transform, ShotDistance, _minShotDistance, _zombiePools);

        while (_building.CurrentSafetyMargin > 0)
        {
            _lastTarget = _searchTarget.GetFurthest();

            if (_lastTarget != null)
            {
                if (_fireCoroutine == null)
                    _fireCoroutine = StartCoroutine(Firing());
            }
            else
            {
                if (_fireCoroutine != null)
                {
                    StopCoroutine(_fireCoroutine);
                    _fireCoroutine = null;
                }
            }

            yield return waitUpdateDelay;
        }

        _waitTargetCoroutine = null;
    }
}
