using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private BoxCollider _boxCollider;

    [SerializeField] private float _damage;
    [SerializeField] private float _radius;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void Explode()
    {
        ParticleSystem.MainModule particleMainModule = _explosionEffect.main;
        particleMainModule.startSizeMultiplier = _radius;
        _explosionEffect.Play();

        Collider[] enemiesColliders = Physics.OverlapSphere(transform.position, _radius, Layers.Enemy);

        foreach (var enemiesCollider in enemiesColliders)
                enemiesCollider.GetComponent<IDamageable>().TakeDamage(_damage);

        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        _meshRenderer.enabled = false;
        _boxCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
