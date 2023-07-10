using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speedMove = 1f;
    [SerializeField] private float _speedRotate = 50f;
    [SerializeField] private CharacterController _characterController;

    private Enemy _enemy;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (_characterController.enabled == false)
            return;

        AnimatorStateInfo animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.IsName(Constants.AnimatorStates.EnemyMove))
            _characterController.Move((_enemy.Target.Position - transform.position).normalized * _speedMove * Time.deltaTime);
        if (animatorStateInfo.IsName(Constants.AnimatorStates.Idle) || animatorStateInfo.IsName(Constants.AnimatorStates.EnemyMove))
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((_enemy.Target.Position - transform.position).With(y: 0)), Time.deltaTime * _speedRotate);
    }
}
