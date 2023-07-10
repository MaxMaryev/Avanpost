using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _targetMaterial;

    private Material _startMaterial;

    private void Start()
    {
        _startMaterial = _meshRenderer.materials[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
            _meshRenderer.material = _targetMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
            _meshRenderer.material = _startMaterial;
    }

}
