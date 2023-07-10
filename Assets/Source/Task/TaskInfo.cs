using Lean.Localization;
using System;
using UnityEngine;

public abstract class TaskInfo : MonoBehaviour
{
    [SerializeField] private string _description;
    [SerializeField] private MonoBehaviour _taskCondition;

    private ITaskCondition _condition => (ITaskCondition)_taskCondition;

    public event Action Updated;

    private void OnValidate()
    {
        if (_taskCondition && !(_taskCondition is ITaskCondition))
        {
            Debug.LogError(nameof(_condition) + " needs to implement " + nameof(ITaskCondition));
            _taskCondition = null;
        }
    }

    private void OnEnable()
    {
        _condition.Updated += OnconditionUpdated;
    }

    private void OnDisable()
    {
        _condition.Updated -= OnconditionUpdated;
    }

    public virtual string ShowInfo()
    {
        return LeanLocalization.CurrentTranslations[_description].Data + ".";
    }

    private void OnconditionUpdated()
    {
        Updated?.Invoke();
    }
}
