using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimationsController))]
public class Player : MonoBehaviour, IShooter, IDamageable, ITargetable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _healingValue;
    [SerializeField] private float _healingCooldown;

    private PlayerAnimationsController _playerAnimationsController;
    private Coroutine _shootingCoroutine;

    public event Action TriedShoot;

    public Health Health { get; private set; }
    public Vector3 Position => transform.position;
    public bool IsAlive => Health.CurrentValue > 0;

    public event Action Died;

    private void Awake()
    {
        _playerAnimationsController = GetComponent<PlayerAnimationsController>();
        Health = new Health(_maxHealth);
        Health.Load();
        BeginHealing();
    }

    public void BeginHealing() => StartCoroutine(Healing());

    public void BeginTryShooting()
    {
        _shootingCoroutine = StartCoroutine(TryShooting());

        IEnumerator TryShooting()
        {
            float delay = 0.05f;

            WaitForSeconds waitForSeconds = new(delay);

            while (IsAlive)
            {
                TriedShoot?.Invoke();
                yield return null;
            }
        }
    }

    public void StopShooting()
    {
        if (_shootingCoroutine != null)
            StopCoroutine(_shootingCoroutine);
    }

    private IEnumerator Healing()
    {
        while (IsAlive)
        {
            yield return new WaitForSeconds(_healingCooldown);

            if (IsAlive && Health.CurrentValue < Health.MaxValue)
                Health.Increase(_healingValue);

            yield return null;
        }
    }

    public void TakeDamage(float damage)
    {
        if (IsAlive == false)
            return;

        Health.Decrease(damage);
        _playerAnimationsController.PlayHitAnimation();
    }
}