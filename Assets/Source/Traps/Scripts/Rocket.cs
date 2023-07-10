using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _point2;
    [SerializeField] private Vector3 _point3;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _damage;
    [SerializeField] private float _damageRadius;
    [SerializeField] private AnimationCurve _speedCurve;
    [SerializeField] private ParticleSystem _trail;

    private Vector3 _targetPosition;
    private float _currentFlyTime;
    private float _flyDuration = 1.5f;

    private void Awake() => _startPosition = transform.position;

    private void Start()
    {
        StartCoroutine(DelayedEnableTrail());
        StartCoroutine(Explode());
    }

    private void Update()
    {
        if (_currentFlyTime >= 1)
            return;

        _currentFlyTime += Time.deltaTime / _flyDuration;

        transform.position = Bezier.GetPoint(_startPosition, _point2, _point3, _targetPosition, _speedCurve.Evaluate(_currentFlyTime));
        transform.rotation = Quaternion.LookRotation(Bezier.GetFirstDerivative(_startPosition, _point2, _point3, _targetPosition, _speedCurve.Evaluate(_currentFlyTime))) * Quaternion.Euler(90, 0, 0);
    }

    public void Init(Vector3 point2, Vector3 point3, Vector3 targetPosition)
    {
        _point2 = point2 + Random.insideUnitSphere;
        _point3 = point3 + Random.insideUnitSphere;
        _targetPosition = targetPosition;
    }

    private IEnumerator Explode()
    {
        yield return new WaitUntil(() => _currentFlyTime >= 1);
        _particleSystem.gameObject.SetActive(true);
        transform.rotation = Quaternion.identity;
        transform.position += Vector3.up;
        _particleSystem.Play();

        Collider[] enemiesColliders = Physics.OverlapSphere(transform.position, _damageRadius, Layers.Enemy);

        foreach (var enemiesCollider in enemiesColliders)
            enemiesCollider.GetComponent<IDamageable>().TakeDamage(_damage);

        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private IEnumerator DelayedEnableTrail()
    {
        yield return new WaitUntil(() => _currentFlyTime >= 0.2f);
        _trail.gameObject.SetActive(true);
    }
}
