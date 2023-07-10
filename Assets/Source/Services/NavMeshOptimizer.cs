using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshOptimizer : MonoBehaviour
{
    [SerializeField] private ZombiesPool _zombiePool;

    private void Start()
    {
        StartCoroutine(Optimizing());
    }

    private IEnumerator Optimizing()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(3);

        while (_zombiePool.enabled)
        {
            NavMesh.pathfindingIterationsPerFrame = 100 + _zombiePool.Enabled.Count * 2;
            yield return waitForSeconds;
        }
    }
}
