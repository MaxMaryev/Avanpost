using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationsController : MonoBehaviour
{
    private Animator _animator;

    public Animator Animator => _animator;

    public bool IsIdle => _animator.GetCurrentAnimatorStateInfo(0).IsName(Constants.AnimatorStates.Idle);
    public bool IsFell => _animator.GetBool(Constants.AnimatorParameters.IsFell);
    public bool IsMove => _animator.GetBool(Constants.AnimatorParameters.IsMove);

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public bool IsCurrentAnimationEnded => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f;

    public void SetStateAttack(bool state)
    {
        _animator.SetBool(Constants.AnimatorParameters.IsAttack, state);
    }

    public void PlayAnimationAttack(int action)
    {
        _animator.SetFloat(Constants.AnimatorParameters.ActionAttack, action);
        SetTrigger(AnimatorTrigger.Attack);
    }

    public void PlayAnimationHit(int action, int weapon)
    {
        _animator.SetFloat(Constants.AnimatorParameters.ActionHit, action);
        _animator.SetFloat(Constants.AnimatorParameters.WeaponIndex, weapon);
        SetTrigger(AnimatorTrigger.Hit);
    }

    public void PlayAnimationDeath(int action)
    {
        _animator.SetBool(Constants.AnimatorParameters.IsDying, true);
        _animator.SetFloat(Constants.AnimatorParameters.ActionDeath, action);
        SetTrigger(AnimatorTrigger.Died);
    }

    public void PlayAnimationMove(int ActionMove)
    {
        _animator.SetFloat(Constants.AnimatorParameters.ActionMove, ActionMove);
        _animator.SetBool(Constants.AnimatorParameters.IsMove, true);
    }

    public void StopAnimationMove()
    {
        _animator.SetBool(Constants.AnimatorParameters.IsMove, false);
    }

    public void SetMoveType()
    {
        _animator.SetFloat(Constants.AnimatorParameters.TypeMove, 1);
        _animator.SetBool(Constants.AnimatorParameters.IsWithoutLegs, true);
        SetTrigger(AnimatorTrigger.BrokeLegs);
    }

    public void PlayAnimationFell(int action, int weapon)
    {
        _animator.SetBool(Constants.AnimatorParameters.IsFell, true);
        _animator.SetFloat(Constants.AnimatorParameters.WeaponIndex, weapon);
        _animator.SetFloat(Constants.AnimatorParameters.ActionFell, action);
        SetTrigger(AnimatorTrigger.Fell);
    }

    public void PlayAnimationBiting()
    {
        SetTrigger(AnimatorTrigger.Biting);
    }

    private void SetTrigger(AnimatorTrigger animatorTrigger)
    {
        _animator.SetInteger(Constants.AnimatorParameters.TriggerNumber, (int)animatorTrigger);
        _animator.SetTrigger(Constants.AnimatorParameters.Trigger);
    }
}
