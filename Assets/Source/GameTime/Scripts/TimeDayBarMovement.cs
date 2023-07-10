using UnityEngine;
using UnityEngine.UI;

public class TimeDayBarMovement : MonoBehaviour
{
    [SerializeField] private RawImage _barImage;
    [SerializeField] private DayCycleManager _dayCycleManager;
    [SerializeField] private float _offsetX;

    private void Update() => _barImage.uvRect = new Rect(_dayCycleManager.CurrentNormalizedTime - _offsetX, _barImage.uvRect.y, _barImage.uvRect.width, _barImage.uvRect.height);
}
