using MTAssets.SkinnedMeshCombiner;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombineHandler : MonoBehaviour
{
    [SerializeField] private CombinedMeshesManager _combinedMeshesManager;
    [SerializeField] private List<SkinnedMeshRenderer> _skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
    [SerializeField] private List<CriticalHit> _criticalHits = new List<CriticalHit>();

    private void OnEnable()
    {
        foreach (var crit in _criticalHits)
            crit.Applied += OnCritApplied;
    }

    private void OnDisable()
    {
        foreach (var crit in _criticalHits)
            crit.Applied -= OnCritApplied;

        _combinedMeshesManager.enabled = true;
        _combinedMeshesManager.gameObject.SetActive(true);

        foreach (var mesh in _skinnedMeshRenderers)
            mesh.gameObject.SetActive(false);
    }

    private void OnCritApplied(CriticalHit criticalHit)
    {
        if (_combinedMeshesManager.enabled == false)
            return;

        _combinedMeshesManager.enabled = false;
        _combinedMeshesManager.gameObject.SetActive(false);

        foreach (var mesh in _skinnedMeshRenderers)
            mesh.gameObject.SetActive(true);
    }
}
