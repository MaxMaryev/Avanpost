using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDestructiblePart
{
    [SerializeField] private List<SkinnedMeshRenderer> _skinnedMeshRenderers;
    [SerializeField] private Transform _bodyPartTransform;
    [SerializeField] private Transform _bloodShowerPoint;
    [SerializeField] private EnemyEffects _enemyEffects;

    public bool CanDeactivate => _skinnedMeshRenderers[0].enabled == true;

    public void Deactivate()
    {
        foreach (var mesh in _skinnedMeshRenderers)
            if (mesh != null)
                mesh.enabled = false;

        _enemyEffects.PlayCriticalHitEffect(_bodyPartTransform, _bloodShowerPoint);
    }

    public void Activate()
    {
        foreach (var mesh in _skinnedMeshRenderers)
            if (mesh != null)
                mesh.enabled = true;
    }
}
