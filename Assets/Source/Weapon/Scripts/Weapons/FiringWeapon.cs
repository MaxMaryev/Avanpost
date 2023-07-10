using System;
using System.Collections;
using UnityEngine;

public class FiringWeapon : Weapon
{
    [SerializeField] private bool _isAutoReloadable;
    
    private Coroutine _reloadingCoroutine;

    [field: SerializeField] public int ClipCapacity { get; private set; }
    [field: SerializeField] public int CurrentBulletsCount { get; protected set; }
    [field: SerializeField] protected float DefaultReloadDuration { get; private set; }
    [field: SerializeField] protected ParticleSystem Bullet { get; private set; }
    [field: SerializeField] protected ParticleSystem ShotEffect { get; private set; }
    [field: SerializeField] protected ParticleSystem BulletShellEffect { get; private set; }

    protected IFriskAnimationsHandler ReloadAnimationsHandler { get; private set; }

    public virtual event Action<float, float> BulletChanged;
    public virtual event Action<FiringWeapon> Reloaded;
    public virtual event Action<FiringWeapon> Fired;
    public virtual event Action<bool> ReloadStateChanged;

    public float TimeAfterLastShot { get; protected set; }
    protected bool IsReloaded => CurrentBulletsCount > 0;

    private void OnEnable()
    {
        TimeAfterLastShot = 0;

        if (CurrentBulletsCount <= 0 && _isAutoReloadable)
            _reloadingCoroutine = StartCoroutine(Reloading(DefaultReloadDuration));
    }

    private void OnDisable()
    {
        if (_reloadingCoroutine != null)
            StopCoroutine(_reloadingCoroutine);
    }

    public void InitReloadAnimationsHandler(IFriskAnimationsHandler reloadAnimationHandler) => ReloadAnimationsHandler = reloadAnimationHandler;
    public void InitCurrentBulletsCount(int count) => CurrentBulletsCount = count;

    private void Update() => TimeAfterLastShot += Time.deltaTime;

    public override void Fire()
    {
        if (TimeAfterLastShot < AttackCooldown || IsReloaded == false)
            return;

        Bullet.Play();
        

        if (BulletShellEffect != null)
            BulletShellEffect.Play();

        if (ShotEffect != null)
            ShotEffect.Play();

        PlayerAnimationsController.PlayFireAnimation();
        CurrentBulletsCount--;
        TimeAfterLastShot = 0;

        BulletChanged?.Invoke(ClipCapacity, CurrentBulletsCount);
        Fired?.Invoke(this);

        if (CurrentBulletsCount <= 0 && _isAutoReloadable)
            _reloadingCoroutine = StartCoroutine(Reloading(DefaultReloadDuration));
    }

    public virtual void ReloadInstanly() => CurrentBulletsCount = ClipCapacity;


    private IEnumerator Reloading(float reloadDuration)
    {
        PlayerAnimationsController.PlayReloadAnimation();
        yield return new WaitForSeconds(reloadDuration);
        CurrentBulletsCount = ClipCapacity;
        PlayerAnimationsController.StopReloadAnimation();
    }
}
