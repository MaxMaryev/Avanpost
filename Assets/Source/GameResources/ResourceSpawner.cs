using System;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _templateResource;

    private Pool<Resource> _pool;

    public event Action<Resource> ResourceSpawned;

    private void Awake()
    {
        _pool = new Pool<Resource>(_templateResource, transform, 5);
    }

    public void SpawnResource(Vector3 startPosition, Vector3 targetPosition, int reward)
    {
        Resource resource = _pool.GetFreeElement();
        resource.Picked += OnResourcePicked;
        resource.transform.position = startPosition;
        resource.gameObject.SetActive(true);
        resource.Init(reward, targetPosition);
        ResourceSpawned?.Invoke(resource);
    }

    private void OnResourcePicked(Resource resource)
    {
        resource.Picked -= OnResourcePicked;
        resource.gameObject.SetActive(false);
    }
}
