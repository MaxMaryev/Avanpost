using System;
using UnityEngine;

public class ResourceSpawnerDeterminant : MonoBehaviour
{
    [SerializeField] private GarbageSpawner _garbageSpawner; 

    public ResourceSpawner GetSpawner(Resource resource)
    {
        ResourceSpawner resourceSpawner = null;

        if (resource is Garbage)
            resourceSpawner = _garbageSpawner;

        return resourceSpawner;
    }
}
