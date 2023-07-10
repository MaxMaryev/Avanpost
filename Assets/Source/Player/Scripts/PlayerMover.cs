using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(PlayerPresenter), typeof(Player))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private InputSystemHandler _inputSystemHandler;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private PlayerPresenter _playerPresenter;
    private Player _player;
    private Quaternion _desiredRotation;
    private QuarterDeterminator _quarterDeterminator;
    private float _accelerationSpeed = 20f;

    public float CurrentSpeed => _navMeshAgent.velocity.magnitude;
    public float MaxSpeed => _navMeshAgent.speed;
    public float ViewAngle { get; private set; } = 15;
    public bool IsMoving { get; private set; }
    public bool IsLookingAtEnemy { get; private set; }
    public int DirectionQuarter { get; private set; }

    private void Awake()
    {
        _playerPresenter = GetComponent<PlayerPresenter>();
        _player = GetComponent<Player>();
        _quarterDeterminator = new QuarterDeterminator();
        _navMeshAgent.updateRotation = false;
    }

    private void Update()
    {
        if (_player.IsAlive == false)
            return;

        _navMeshAgent.velocity = Vector3.MoveTowards(_navMeshAgent.velocity, _inputSystemHandler.GetDirection() * _navMeshAgent.speed, Time.deltaTime * _accelerationSpeed);
        IsMoving = _navMeshAgent.velocity.sqrMagnitude > Constants.Epsilon;
        _desiredRotation = GetDesiredRotation();
    }

    public void LookForward()
    {
        if (_navMeshAgent.velocity != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_navMeshAgent.velocity), _rotationSpeed);
    }

    public void LookAtEnemy()
    {
        DirectionQuarter = _quarterDeterminator.GetQuarter(_navMeshAgent.velocity);
        IsLookingAtEnemy = Quaternion.Angle(transform.rotation, _desiredRotation) < ViewAngle;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _desiredRotation, _rotationSpeed);
    }

    private Quaternion GetDesiredRotation()
    {
        if (_playerPresenter.Target == null)
            return transform.rotation;

        var targetXZ = _playerPresenter.Target.Position.With(y: 0);
        var objectPostionXZ = transform.position.With(y: 0);
        return Quaternion.LookRotation(targetXZ - objectPostionXZ);
    }
}