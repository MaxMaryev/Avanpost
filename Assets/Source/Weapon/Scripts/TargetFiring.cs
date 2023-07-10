using UnityEngine;

public class TargetFiring : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;

    public void Fire()
    {
        Instantiate(_effect, transform.position, Quaternion.LookRotation(Vector3.up));
    }
}
