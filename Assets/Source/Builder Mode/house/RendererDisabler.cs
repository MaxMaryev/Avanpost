using System.Collections.Generic;
using UnityEngine;

public class RendererDisabler : MonoBehaviour
{
    [SerializeField] private List<Renderer> _roofRenderer = new List<Renderer>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
            foreach (var roof in _roofRenderer)
                roof.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
            foreach (var roof in _roofRenderer)
                roof.enabled = true;
    }
}
