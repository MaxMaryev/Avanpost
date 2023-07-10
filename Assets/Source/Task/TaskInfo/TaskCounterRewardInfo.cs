using Lean.Localization;
using UnityEngine;

public class TaskCounterRewardInfo : TaskCounterInfo
{
    [SerializeField] private TaskReward _taskReward;

    public int RewardAmount => _taskReward.GetAmountReward();

    public override string ShowInfo()
    {
        return base.ShowInfo() + $" ({LeanLocalization.CurrentTranslations["TaskReward"].Data}: {RewardAmount})";
    }
}
