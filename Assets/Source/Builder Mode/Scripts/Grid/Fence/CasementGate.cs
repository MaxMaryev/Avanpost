using System.Collections;
using UnityEngine;

public class CasementGate : Gate
{
    [SerializeField] private Transform _longRightDoor;
    [SerializeField] private Transform _longLeftDoor;
    [SerializeField] private Transform _shortRightDoor;

    private Coroutine _openningCoroutine;
    private Coroutine _closingCoroutine;

    protected override void GateLockOpened(BoxCollider collider)
    {
        if (_closingCoroutine != null)
            StopCoroutine(_closingCoroutine);

        collider.enabled = false;

        if (IsDiagonally)
            _openningCoroutine = StartCoroutine(OpenLongDoors());
        else
            _openningCoroutine = StartCoroutine(OpenShortDoor());
    }

    protected override void GateLockClosed(BoxCollider collider)
    {
        if (_openningCoroutine != null)
            StopCoroutine(_openningCoroutine);

        collider.enabled = true;

        if (IsDiagonally)
            _closingCoroutine = StartCoroutine(CloseLongDoors());
        else
            _closingCoroutine = StartCoroutine(CloseShortDoor());
    }

    private IEnumerator OpenShortDoor()
    {
        Vector3 rightDoorTarget = new Vector3(0f, 100, 0f);

        while (_shortRightDoor.localEulerAngles.y < rightDoorTarget.y)
        {
            _shortRightDoor.localEulerAngles = Vector3.MoveTowards(_shortRightDoor.localEulerAngles, rightDoorTarget, Time.deltaTime * 200);

            yield return null;
        }
    }

    private IEnumerator CloseShortDoor()
    {
        Vector3 rightDoorTarget = new Vector3(0f, 0, 0f);

        while (_shortRightDoor.localEulerAngles.y > rightDoorTarget.y)
        {
            _shortRightDoor.localEulerAngles = Vector3.MoveTowards(_shortRightDoor.localEulerAngles, rightDoorTarget, Time.deltaTime * 200);

            yield return null;
        }
    }

    private IEnumerator OpenLongDoors()
    {
        Vector3 leftDoorTarget = new Vector3(0f, -115, 0f);
        Vector3 rightDoorTarget = new Vector3(0f, 115, 0f);

        while (_longRightDoor.localEulerAngles.y < rightDoorTarget.y)
        {
            _longLeftDoor.localEulerAngles = Vector3.MoveTowards(_longLeftDoor.localEulerAngles, leftDoorTarget, Time.deltaTime * 200);
            _longRightDoor.localEulerAngles = Vector3.MoveTowards(_longRightDoor.localEulerAngles, rightDoorTarget, Time.deltaTime * 200);

            yield return null;
        }
    }

    private IEnumerator CloseLongDoors()
    {
        Vector3 leftDoorTarget = new Vector3(0f, 0, 0f);
        Vector3 rightDoorTarget = new Vector3(0f, 0, 0f);

        while (_longRightDoor.localEulerAngles.y > rightDoorTarget.y)
        {
            _longLeftDoor.localEulerAngles = Vector3.MoveTowards(_longLeftDoor.localEulerAngles, leftDoorTarget, -Time.deltaTime * 200);
            _longRightDoor.localEulerAngles = Vector3.MoveTowards(_longRightDoor.localEulerAngles, rightDoorTarget, Time.deltaTime * 200);

            yield return null;
        }
    }
}
