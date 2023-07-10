using UnityEngine;

public class Lamp : MonoBehaviour
{
    [field: SerializeField] public Light Light { get; private set; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyLightInfluence enemy))
            enemy.IncreaseLigthsCount();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EnemyLightInfluence enemy))
            enemy.DecreaseLigthsCount();
    }
}
