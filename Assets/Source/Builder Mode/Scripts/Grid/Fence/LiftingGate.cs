using System;
using System.Collections;
using UnityEngine;

public class LiftingGate : Gate
{
    [SerializeField] private Transform _shortDoor;
    [SerializeField] private Transform _longDoor;
    [SerializeField] private Transform _shortCounterweight;
    [SerializeField] private Transform _longCounterweight;

    private Coroutine _openningCoroutine;
    private Coroutine _closingCoroutine;
    private float _doorTargetHeight = -0.3f;
    private float _defaultHeight;

    private delegate bool StopMoveCondition();
    private StopMoveCondition _stopMoveCondition;
    private Transform _door;
    private Transform _counterweight;

    private void Awake() => _defaultHeight = _shortDoor.position.y;

    protected override void GateLockOpened(BoxCollider collider)
    {
        if (_closingCoroutine != null)
            StopCoroutine(_closingCoroutine);

        collider.enabled = false;
        DefineModel();

        _stopMoveCondition = () => _door.position.y < _doorTargetHeight;
        _openningCoroutine = StartCoroutine(MoveDoor(_door, _counterweight, _doorTargetHeight, _stopMoveCondition));
    }

    protected override void GateLockClosed(BoxCollider collider)
    {
        if (_openningCoroutine != null)
            StopCoroutine(_openningCoroutine);

        collider.enabled = true;
        DefineModel();

        _stopMoveCondition = () => _door.position.y > _defaultHeight;
        _closingCoroutine = StartCoroutine(MoveDoor(_door, _counterweight, _defaultHeight, _stopMoveCondition));
    }

    private IEnumerator MoveDoor(Transform door, Transform counterweight, float height, StopMoveCondition condition)
    {
        Vector3 doorTargetPosition = door.position.With(y: height);

        while (condition.Invoke())
        {
            door.position = Vector3.MoveTowards(door.position, doorTargetPosition, Time.deltaTime * 4);
            counterweight.localPosition = door.localPosition.With(y: -door.localPosition.y / 2.2f);
            yield return null;
        }
    }

    private void DefineModel()
    {
        if (IsDiagonally)
        {
            _counterweight = _longCounterweight;
            _door = _longDoor;
        }
        else
        {
            _counterweight = _shortCounterweight;
            _door = _shortDoor;
        }
    }
}
