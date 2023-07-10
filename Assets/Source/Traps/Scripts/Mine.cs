using System;
using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour, IExplodable
{
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private ParticleSystem _flashingEffect;
    [SerializeField] private float _damage;
    [SerializeField] private float _radius;
    [SerializeField] private Building _building;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Light _light;

    private Coroutine _explosionCoroutine;

    public Transform Transform => transform;

    public event Action<IExplodable> Exploded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
            if (_explosionCoroutine == null)
                _explosionCoroutine = StartCoroutine(DelayedExplosion());
    }

    private void Start() => _flashingEffect.Play();

    private IEnumerator DelayedExplosion()
    {
        ParticleSystem.MainModule explosionParticleMainModule = _explosionEffect.main;
        explosionParticleMainModule.startSizeMultiplier = _radius;

        _flashingEffect.Stop();
        yield return new WaitUntil(() => _flashingEffect.isStopped);
        ParticleSystem.MainModule flashingParticleMainModule = _flashingEffect.main;
        flashingParticleMainModule.duration /= 5;
        flashingParticleMainModule.startLifetime = 0.1f;
        _flashingEffect.Play();

        float exposionDelay = 0.5f;
        yield return new WaitForSeconds(exposionDelay);
        _renderer.enabled = false;
        _flashingEffect.Stop();
        _explosionEffect.Play();
        Exploded?.Invoke(this);
        _light.gameObject.SetActive(true);

        Collider[] enemiesColliders = Physics.OverlapSphere(transform.position, _radius, Layers.Enemy);

        foreach (var enemiesCollider in enemiesColliders)
        {
            IWeaponVisitor weaponVisitor = enemiesCollider.GetComponent<IWeaponVisitor>();

            if (weaponVisitor != null)
                weaponVisitor.Visit(this, _damage);
        }
           

        yield return new WaitForSeconds(explosionParticleMainModule.duration);

        if (_building.BuildingsSaver != null)
            _building.BuildingsSaver.DeleteFromSaveList(_building);

        Destroy(gameObject);
    }
}
