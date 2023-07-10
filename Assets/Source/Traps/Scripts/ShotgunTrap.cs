using System;
using System.Collections;
using UnityEngine;

public class ShotgunTrap : FiringWeapon
{
    [SerializeField] private Building _building;
    [SerializeField] private OnTriggerHandler _reloadTrigger;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _upperPart;
    [SerializeField] private Transform _aim;

    private bool _isPlayerNear;
    private Coroutine _waitTargetCoroutine;
    private Coroutine _fireCoroutine;
    private ITargetable _lastTarget;
    private ObjectsPool[] _zombiePools;
    private SearchTarget _searchTarget;
    private bool _isReload;

    public override event Action<float, float> BulletChanged;
    public override event Action<FiringWeapon> Reloaded;
    public override event Action<FiringWeapon> Fired;
    public override event Action<bool> ReloadStateChanged;

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

    private void Update()
    {
        if (_lastTarget != null && CurrentBulletsCount > 0 && _isPlayerNear == false)
        {
            Vector3 direction = _upperPart.position - _lastTarget.Position.With(y: _upperPart.transform.position.y);
            _upperPart.transform.rotation = Quaternion.RotateTowards(_upperPart.transform.rotation,
                Quaternion.LookRotation(direction, Vector3.up), _rotationSpeed * Time.deltaTime);
        }
    }

    public void InitZombiesPool(params ObjectsPool[] zombiesPool) => _zombiePools = zombiesPool;

    public override void Accept(IWeaponVisitor weaponVisitor, float damage)
    {
        weaponVisitor.Visit(this, damage);
    }

    public override void ReloadInstanly()
    {
        base.ReloadInstanly();
        BulletChanged.Invoke(ClipCapacity, CurrentBulletsCount);
    }

    private IEnumerator WaitingTarget()
    {
        float updateDelay = 0.3f;
        WaitForSeconds waitUpdateDelay = new WaitForSeconds(updateDelay);
        yield return new WaitUntil(() => _zombiePools != null);
        _searchTarget = new SearchTarget(transform, ShotDistance, 0, _zombiePools);

        while (_building.CurrentSafetyMargin > 0)
        {
            _lastTarget = _searchTarget.GetNearest();

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

    private bool IsTargetOnFireLine()
    {
        Ray ray = new Ray(_aim.position, _aim.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, Layers.Enemy))
            if (hit.collider != null)
                return true;

        return false;
    }

    private IEnumerator Firing()
    {
        while (CurrentBulletsCount > 0 && IsTargetOnFireLine() && _isReload == false)
        {
            Bullet.Play();
            ShotEffect.Play();
            BulletShellEffect.Play();
            Fired?.Invoke(this);
            CurrentBulletsCount--;
            BulletChanged?.Invoke(ClipCapacity, CurrentBulletsCount);
            yield return new WaitForSeconds(AttackCooldown);
        }

        _fireCoroutine = null;
    }

    private void OnReloadTriggerEntered(Collider other)
    {
        if (_building.IsDestroying)
            return;

        if (CurrentBulletsCount == ClipCapacity)
            return;

        if (other.TryGetComponent(out PlayerMover playerMover))
        {
            _isPlayerNear = true;
            StartCoroutine(Reload(playerMover));

            if (_waitTargetCoroutine == null)
                StartCoroutine(WaitingTarget());
        }
    }

    private void OnReloadTriggerExited(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _isPlayerNear = false;
            ReloadAnimationsHandler.StopAnimationFrisk();
        }
    }

    private IEnumerator Reload(PlayerMover playerMover)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(DefaultReloadDuration / ClipCapacity);

        while (_isPlayerNear && CurrentBulletsCount < ClipCapacity)
        {
            yield return waitForSeconds;

            if (playerMover.IsMoving)
            {
                ReloadAnimationsHandler.StopAnimationFrisk();
                _isReload = false;
                ReloadStateChanged?.Invoke(_isReload);
            } 
            else
            {
                CurrentBulletsCount++;
                ReloadAnimationsHandler.PlayAnimationFrisk();
                BulletChanged?.Invoke(ClipCapacity, CurrentBulletsCount);
                _isReload = true;
                ReloadStateChanged?.Invoke(_isReload);
            }

            if (CurrentBulletsCount == ClipCapacity)
                Reloaded?.Invoke(this);
        }

        ReloadAnimationsHandler.StopAnimationFrisk();
        _isReload = false;
        ReloadStateChanged?.Invoke(_isReload);

        if (_waitTargetCoroutine == null)
            _waitTargetCoroutine = StartCoroutine(WaitingTarget());
    }
}
