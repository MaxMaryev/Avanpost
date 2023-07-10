using System.Collections;
using UnityEngine;

public class BoardedDefence : MonoBehaviour
{
    [SerializeField] private Transform _overlapCenter;
    [SerializeField] private BoxCollider _boxCollider;

    private IBreakable[] _boards;
    private bool _isPlayerNear;

    public bool IsBroken { get; private set; }
    public bool IsRepaired { get; private set; }

    private void Awake() => _boards = GetComponentsInChildren<Board>();

    private void Start() => StartCoroutine(Breaking());

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_overlapCenter.position, transform.localScale);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _isPlayerNear = true;
            StartCoroutine(Repairing());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _isPlayerNear = false;

            foreach (var board in _boards)
                if (board.IsRepaired == false)
                    board.SetKinematic(true);
        }
    }

    private IEnumerator Repairing()
    {
        int repairedBoardsCount = 0;

        foreach (var board in _boards)
            if (board.IsRepaired == false)
                board.SetKinematic(true);

        while (IsRepaired == false && _isPlayerNear)
        {
            foreach (var board in _boards)
            {
                if (board.IsRepaired)
                {
                    repairedBoardsCount++;
                    board.SetKinematic(false);
                    continue;
                }
                else
                {
                    board.Repair();
                }
            }

            if (repairedBoardsCount == _boards.Length)
            {
                IsRepaired = true;
                _boxCollider.isTrigger = false;
            }

            yield return null;
        }
    }

    private IEnumerator Breaking()
    {
        int brokenBoardsCount;
        float delay = 1;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        while (IsBroken == false)
        {
            brokenBoardsCount = 0;
            Collider[] colliders = Physics.OverlapBox(_overlapCenter.position, transform.localScale / 2, transform.rotation, Layers.Enemy);

            if (colliders.Length != 0)
            {
                foreach (var board in _boards)
                {
                    if (board.IsBroken)
                    {
                        brokenBoardsCount++;
                        continue;
                    }
                    else
                    {
                        board.Break(colliders.Length);
                        break;
                    }
                }

                if (brokenBoardsCount == _boards.Length)
                {
                    IsBroken = true;
                    _boxCollider.isTrigger = true;
                }
            }

            yield return waitForSeconds;
        }
    }
}
