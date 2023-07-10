using TMPro;
using UnityEngine;

public class TaskRewardPanelView : MonoBehaviour
{
    [SerializeField] private TMP_Text _amountText;

    private ITaskable _taskable;

    public void Init(ITaskable taskable)
    {
        _taskable = taskable;

        if (taskable.TaskReward.GetAmountReward() > 0)
        {
            _amountText.text = taskable.TaskReward.GetAmountReward().ToString();
            gameObject.SetActive(true);
        }
    }
}
