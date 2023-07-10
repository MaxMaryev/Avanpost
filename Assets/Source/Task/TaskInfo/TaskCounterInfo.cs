using Lean.Localization;
using UnityEngine;

public class TaskCounterInfo : TaskInfo
{
    [SerializeField] private MonoBehaviour _counter;

    private ICountable _countable => (ICountable)_counter;

    private void OnValidate()
    {
        if (_counter && !(_counter is ITaskCondition))
        {
            Debug.LogError(nameof(_counter) + " needs to implement " + nameof(ITaskCondition));
            _counter = null;
        }
    }


    public override string ShowInfo()
    {
        return base.ShowInfo() + " " + $"{LeanLocalization.CurrentTranslations["CompletedTask"].Data}: {_countable.CurrentNumber}/{_countable.TargetNumber}";
    }
}
