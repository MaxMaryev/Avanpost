using UnityEngine;

public class TargetFinder
{
    private ObjectsPool[] _pools;

    public TargetFinder(params ObjectsPool[] pools) => _pools = pools;

    public ITargetable TryFindNearestTo(Transform to)
    {
        ITargetable nearest = null;

        for (int i = 0; i < _pools.Length; i++)
        {
            if (_pools[i].Enabled.Count == 0)
                continue;

            if (nearest == null)
                nearest = _pools[i].Enabled[0];

            float distanceToLastNearest = (nearest.Position - to.position).SqrMagnitudeXZ();

            for (int j = 0; j < _pools[i].Enabled.Count; j++)
            {
                float distanceToCurrent = (_pools[i].Enabled[j].Position - to.position).SqrMagnitudeXZ();

                if (distanceToCurrent <= distanceToLastNearest)
                {
                    nearest = _pools[i].Enabled[j];
                    distanceToLastNearest = distanceToCurrent;
                }
            }
        }

        return nearest;
    }
}
