using System.Collections.Generic;
using UnityEngine;

public class EnemyEffects : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private EnemyHitHandler _enemyHitHandler;
    [SerializeField] private ParticleSystem _hitBlood;
    [SerializeField] private ParticleSystem _bloodExplosion;
    [SerializeField] private ParticleSystem _bloodSplatDirectional;
    [SerializeField] private ParticleSystem _bloodShower;
    [SerializeField] private List<Renderer> _renderers;

    private void OnEnable()
    {
        foreach (var renderer in _renderers)
            renderer.material.color = Color.white;

        _enemyHitHandler.Hit += OnHit;
    }

    private void OnDisable() => _enemyHitHandler.Hit -= OnHit;

    public void PlayCriticalHitEffect(Transform bodyPartTransform, Transform bloodShowerPoint)
    {
        SetPosition(_bloodShower, _bloodExplosion, _bloodSplatDirectional);
        StartAnimation(_bloodShower, _bloodExplosion, _bloodSplatDirectional);
        SetParent(_bloodShower, _bloodExplosion, _bloodSplatDirectional);

        void SetPosition(ParticleSystem bloodShower, params ParticleSystem[] effects)
        {
            foreach (var effect in effects)
                effect.transform.position = bodyPartTransform.position;

            bloodShower.transform.position = bloodShowerPoint.position;
            _bloodShower.transform.rotation = bloodShowerPoint.rotation;
        }

        void SetParent(ParticleSystem bloodShower, params ParticleSystem[] effects)
        {
            foreach (var effect in effects)
                effect.transform.SetParent(effect.transform);

            bloodShower.transform.SetParent(bloodShowerPoint);
        }

        void StartAnimation(ParticleSystem bloodShower, params ParticleSystem[] effects)
        {
            foreach (var effect in effects)
                effect.Play();

            bloodShower.Play(bloodShowerPoint);
        }
    }

    private void OnHit(WeaponType weaponType)
    {
        if (weaponType != WeaponType.Flamethrower)
            _hitBlood.Play();

        foreach (var renderer in _renderers)
            renderer.material.color = Color.Lerp(renderer.material.color, Color.black, Time.deltaTime);
    }
}
