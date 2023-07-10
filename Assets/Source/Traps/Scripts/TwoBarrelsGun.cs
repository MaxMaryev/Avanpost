using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class TwoBarrelsGun : FiringWeapon
{
    [SerializeField] private Building _building;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _maxAngle;
    [SerializeField] private Transform _leftBarrelShotPoint;
    [SerializeField] private Transform _rightBarrelShotPoint;
    [SerializeField] private Transform _upperPart;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _leftBarrel;
    [SerializeField] private Transform _rightBarrel;
    [SerializeField] private OnTriggerHandler _reloadTrigger;
    [SerializeField] private List<ParticleSystem> _effectBulletShells = new List<ParticleSystem>();

    private bool _isReload;
    private bool _isLeftBarrelShot;
    private bool _isPlayerNear;
    private Coroutine _fireCoroutine;
    private Coroutine _waitTargetCoroutine;
    private ObjectsPool[] _zombiePools;
    private SearchTarget _searchTarget;
    private ITargetable _lastTarget;

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
            _upperPart.transform.rotation = Quaternion.RotateTowards(_upperPart.transform.rotation,
            Quaternion.LookRotation(_lastTarget.Position.With(y: _upperPart.transform.position.y) - _upperPart.position, Vector3.up),
            _rotationSpeed * Time.deltaTime);
        }
    }

    public override void Accept(IWeaponVisitor weaponVisitor, float damage)
    {
        weaponVisitor.Visit(this, damage);
    }

    public void InitZombiesPool(params ObjectsPool[] zombiePools) => _zombiePools = zombiePools;

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

        _isReload = false;
        ReloadStateChanged?.Invoke(_isReload);
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

        if (other.TryGetComponent(out PlayerMover playerMover))
        {
            _isPlayerNear = true;
            StartCoroutine(Reload(playerMover));

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

    private IEnumerator Firing()
    {
        while (CurrentBulletsCount > 0 && _isReload == false)
        {
            Bullet.Play();
            ShotEffect.Play();

            foreach (var bulletsShells in _effectBulletShells)
                bulletsShells.Play();

            CurrentBulletsCount--;
            BulletChanged?.Invoke(ClipCapacity, CurrentBulletsCount);
            Fired?.Invoke(this);

            if (_isLeftBarrelShot)
                ChangeBarrel(currentBarrel: _leftBarrel, nextShotPoint: _rightBarrelShotPoint);
            else
                ChangeBarrel(currentBarrel: _rightBarrel, nextShotPoint: _leftBarrelShotPoint);

            _isLeftBarrelShot = !_isLeftBarrelShot;
            yield return new WaitForSeconds(AttackCooldown);
        }

        _fireCoroutine = null;
    }

    private void ChangeBarrel(Transform currentBarrel, Transform nextShotPoint)
    {
        currentBarrel.transform.DOLocalMoveZ(-0.05f, 0.2f).OnComplete(() => currentBarrel.transform.DOLocalMoveZ(0, 0.2f));
        Bullet.transform.position = nextShotPoint.position;
        ShotEffect.transform.position = nextShotPoint.position;
    }
}
