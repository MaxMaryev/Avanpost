using UnityEngine;

public class EnemyStartLoadingScene : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hit;
    [SerializeField] private Collider _collider;
    [SerializeField] private Animator _animator;

    private const string IsDeath = "IsDeath";

    private void OnParticleCollision(GameObject other)
    {
        _hit.Play();
        _animator.SetTrigger(IsDeath);
        _collider.enabled = false;
    }
}
