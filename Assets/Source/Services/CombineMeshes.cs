using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineMeshes : MonoBehaviour
{
    [SerializeField] private MeshFilter[] meshFilters;
    [SerializeField] private bool _isNeedCorrectPosition;
    [SerializeField] private Vector3 _offset;

    void Start()
    {
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

        if (_isNeedCorrectPosition)
            transform.localPosition = _offset;

        transform.gameObject.SetActive(true);

    }
}
