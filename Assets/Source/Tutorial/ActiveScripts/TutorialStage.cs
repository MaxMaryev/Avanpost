using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialStage : MonoBehaviour
{
    [SerializeField] private List<GameObject> _disableObjects = new List<GameObject>();

    public abstract event Action<TutorialStage> Completed;
    public event Action Entered;
    public event Action Exited;

    private void OnEnable() => DeactivateObjects();

    public void Enter()
    {
        OnEnter();
        Entered?.Invoke();
    }

    public void Exit()
    {
        OnExit();
        Exited?.Invoke();
    }

    protected abstract void OnEnter();
    protected abstract void OnExit();

    private void DeactivateObjects()
    {
        foreach (var disObj in _disableObjects)
            disObj.SetActive(false);
    }
}
