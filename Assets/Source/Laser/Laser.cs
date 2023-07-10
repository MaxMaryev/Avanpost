using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position + transform.forward * 200);
        RaycastHit raycastHit;

        if (Physics.Raycast(transform.position, transform.forward, out raycastHit))
        {
            if (raycastHit.collider)
                _lineRenderer.SetPosition(1, raycastHit.point);
        }
    }
}
