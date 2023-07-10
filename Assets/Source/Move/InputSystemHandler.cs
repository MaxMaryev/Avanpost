using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class InputSystemHandler : MonoBehaviour
{
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private List<Image> _joystickImages;

    public Vector3 GetDirection()
    {
        if (_joystick.Input == Vector2.zero)
            return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        else
            return new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
    }
}