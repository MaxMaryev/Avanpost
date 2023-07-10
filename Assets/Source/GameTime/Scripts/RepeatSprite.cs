using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepeatSprite : MonoBehaviour
{
    [SerializeField] private List<Image> _images = new List<Image>();
    [SerializeField] private DayCycleManager _dayCycleManager;
}
