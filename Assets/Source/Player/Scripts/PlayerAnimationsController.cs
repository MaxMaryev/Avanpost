using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour, IRepairAnimationsHandler, IFriskAnimationsHandler
{
    [SerializeField] private InputSystemHandler _inputSystemHandler;
    [SerializeField] private Animator _animator;

    private Player _player;
    private PlayerPresenter _playerPresenter;
    private PlayerMover _playerMover;
    private float _speedTransitionNextAnimationForBlendTree = 10f;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerPresenter = GetComponent<PlayerPresenter>();
        _playerMover = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        if (_player.IsAlive == false)
        {
            PlayDeathAnimation();
            enabled = false;
            return;
        }

        Vector3 joystickDirection = new Vector3(_inputSystemHandler.GetDirection().x, 0, _inputSystemHandler.GetDirection().z);
        Vector3 localDirection = joystickDirection;

        if (_playerPresenter.Target != null)
        {
            Vector3 targetDirection = (_playerPresenter.Target.Position - transform.position).With(y: 0);
            float targetJoystickAngle = Vector3.SignedAngle(targetDirection, joystickDirection, Vector3.up);
            localDirection = new Vector3(Mathf.Sin(Mathf.Deg2Rad * targetJoystickAngle), 0, Mathf.Cos(Mathf.Deg2Rad * targetJoystickAngle));
        }

        float clampDirectionMagnitude = Mathf.Clamp01(_inputSystemHandler.GetDirection().magnitude);

        _animator.SetFloat(Constants.AnimatorParameters.Horizontal, localDirection.x * clampDirectionMagnitude);
        _animator.SetFloat(Constants.AnimatorParameters.Vertical, localDirection.z * clampDirectionMagnitude);
        _animator.SetFloat(Constants.AnimatorParameters.Speed, _playerMover.CurrentSpeed / _playerMover.MaxSpeed);
        _animator.SetFloat(Constants.AnimatorParameters.AimToTarget, Mathf.Lerp(_animator.GetFloat(Constants.AnimatorParameters.AimToTarget), _playerPresenter.IsOnShotDistance ? 1 : 0, Time.deltaTime * _speedTransitionNextAnimationForBlendTree));
        _animator.SetBool(Constants.AnimatorParameters.CanShoot, _playerPresenter.CanShoot);

    }

    public void PlayDeathAnimation()
    {
        _animator.SetBool(Constants.AnimatorParameters.CanShoot, false);
        _animator.CrossFade(Constants.AnimatorStates.Dying, 0.05f, 0);
    }

    public void PlayHitAnimation()
    {
        if (_player.IsAlive == false)
            return;

        _animator.SetInteger(Constants.AnimatorParameters.TriggerNumber, (int)AnimatorTrigger.Hit);
        _animator.SetTrigger(Constants.AnimatorParameters.Trigger);
    }

    public void PlayFireAnimation()
    {
        _animator.SetInteger(Constants.AnimatorParameters.TriggerNumber, (int)AnimatorTrigger.Fire);
        _animator.SetTrigger(Constants.AnimatorParameters.Trigger);
    }

    public void PlayAttackAnimation()
    {
        _animator.SetInteger(Constants.AnimatorParameters.TriggerNumber, (int)AnimatorTrigger.Attack);
        _animator.SetTrigger(Constants.AnimatorParameters.Trigger);
    }

    public void PlayReloadAnimation()
    {
        _animator.SetBool(Constants.AnimatorParameters.IsReload, true);
        _animator.SetTrigger(Constants.AnimatorParameters.Trigger);
    }

    public void StopReloadAnimation()
    {
        _animator.SetBool(Constants.AnimatorParameters.IsReload, false);
    }

    public void PlayWeaponSwitch(WeaponType weaponType)
    {
        _animator.SetFloat(Constants.AnimatorParameters.WeaponIndex, (int)weaponType);
    }

    public void PlayRepairAnimation()
    {
        _animator.SetBool(Constants.AnimatorParameters.IsRepairing, true);
    }

    public void StopRepairAnimation()
    {
        _animator.SetBool(Constants.AnimatorParameters.IsRepairing, false);
    }

    public void PlayAnimationFrisk()
    {
        _animator.SetBool(Constants.AnimatorParameters.IsFrisk, true);
    }

    public void StopAnimationFrisk()
    {
        if (_animator != null)

            _animator.SetBool(Constants.AnimatorParameters.IsFrisk, false);
    }
}
