using UnityEngine;

public class VisualClockController : MonoBehaviour 
{
	[SerializeField] private DayCycleManager _dayCycleManager;
	[SerializeField] private DayAndNightControl _dayAndNightControl;
    [SerializeField] private Transform _hourHand;
    [SerializeField] private Transform _minuteHand;

	private float hoursToDegrees = 360f / 12f;
	private float minsToDegrees = 360f / 60f;

	void Update () 
	{
		float currHour = 24 * _dayCycleManager.CurrentNormalizedTime;
		float currMin = 60 * (currHour - Mathf.Floor (currHour));

		_hourHand.localRotation = Quaternion.Euler (0, 0, -currHour * hoursToDegrees);
		_minuteHand.localRotation = Quaternion.Euler (0, 0, -currMin * minsToDegrees);
	}
}
