using System.Collections.Generic;
using UnityEngine;

public class Pocket : MonoBehaviour
{
    [SerializeField] private List<Resource> _resources = new List<Resource>();
    [SerializeField] private int _chanceOfFalling;
    [SerializeField] private int _minAmount;
    [SerializeField] private int _maxAmount;

    public IReadOnlyList<Resource> Resources => _resources;

    public bool TryGetResource(out ResourceBox resourceBox)
    {
        int chance = Random.Range(0, 100);
        resourceBox = null;

        if (chance <= _chanceOfFalling)
            resourceBox = new ResourceBox(_resources[Random.Range(0, _resources.Count - 1)], Random.Range(_minAmount, _maxAmount));

        return resourceBox != null;
    }
}
