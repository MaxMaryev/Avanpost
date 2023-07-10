using System.Linq;
using UnityEngine;

public class SearchTarget
{
    private Transform _transform;
    private float _maxDistanceToTarget;
    private float _minDistanceToTarget;
    private ObjectsPool[] _objectsPools;

    public SearchTarget(Transform transform, float maxDistance, float minDistance, params ObjectsPool[] pools)
    {
        _transform = transform;
        _maxDistanceToTarget = maxDistance;
        _minDistanceToTarget = minDistance;
        _objectsPools = pools;
    }

    public IVisible GetNearest()
    {
        IVisible nearestTarget = null;

        foreach (var objectPool in _objectsPools)
        {
            if (objectPool.Enabled.Count > 0)
            {
                nearestTarget = objectPool.Enabled.Where(x => x.IsAlive && x.IsVisible && (_transform.position - x.Position).SqrMagnitudeXZ() <= _maxDistanceToTarget).FirstOrDefault();

                if (nearestTarget == null)
                    return null;

                float lastDistance = (_transform.position - nearestTarget.Position).SqrMagnitudeXZ();

                foreach (var target in objectPool.Enabled)
                {
                    if (target.IsAlive == false || target.IsVisible == false)
                        continue;

                    float _distanceToTarget = (_transform.position - target.Position).SqrMagnitudeXZ();

                    if (_distanceToTarget < lastDistance && _distanceToTarget <= _maxDistanceToTarget)
                    {
                        lastDistance = _distanceToTarget;
                        nearestTarget = target;
                    }
                }
            }
        }

        return nearestTarget;
    }

    public IVisible GetFurthest()
    {
        IVisible furthestTarget = null;

        foreach (var objectPool in _objectsPools)
        {
            if (objectPool.Enabled.Count > 0)
            {
                furthestTarget = objectPool.Enabled.Where(x => x.IsAlive && x.IsVisible && 
                (_transform.position - x.Position).SqrMagnitudeXZ() <= _maxDistanceToTarget &&
                (_transform.position - x.Position).SqrMagnitudeXZ() > _minDistanceToTarget).FirstOrDefault();

                if (furthestTarget == null)
                    return null;

                float lastDistance = (_transform.position - furthestTarget.Position).SqrMagnitudeXZ();

                foreach (var target in objectPool.Enabled)
                {
                    if (target.IsAlive == false || target.IsVisible == false)
                        continue;

                    float _distanceToTarget = (_transform.position - target.Position).SqrMagnitudeXZ();

                    if (_distanceToTarget > lastDistance && _distanceToTarget <= _maxDistanceToTarget)
                    {
                        lastDistance = _distanceToTarget;
                        furthestTarget = target;
                    }
                }
            }
        }

        return furthestTarget;
    }
}
