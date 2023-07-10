using System.Collections;
using UnityEngine;

public class PlayerVisibleArea : Lamp
{
    [SerializeField] private LightHandler _lightHandler;
    [SerializeField] private SphereCollider _sphereCollider;

    private void Start()
    {
        //_lightHandler.DeleteLight(Light);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HouseLamp houseLamp))
            _lightHandler.DeleteLight(Light);

        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out HouseLamp houseLamp))
            _lightHandler.AddLight(Light);

        base.OnTriggerExit(other);
    }
}
