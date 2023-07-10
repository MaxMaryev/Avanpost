using System;
using System.Collections.Generic;
using UnityEngine;

public class ReloadShotgunTrapStage : TutorialStage
{
    [SerializeField] private ParticleSystem _template;
    [SerializeField] private TimeDay _toTimeDay;
    [SerializeField] private HintHandler _hintHandler;
    [SerializeField] private string _message;
    [SerializeField] private ContainersHandler _containersHandler;
    [SerializeField] private ArrowIndicates _templateArrow;
    [SerializeField] private TaskBarHandler _taskBarHandler;
    [SerializeField] private Task _task;

    private ITaskActivable _taskActivable => _task;
    private Dictionary<FiringWeapon, ParticleSystem> _reloadTraps = new Dictionary<FiringWeapon, ParticleSystem>();
    private Dictionary<FiringWeapon, ArrowIndicates> _arrowIndicateTraps = new Dictionary<FiringWeapon, ArrowIndicates>();

    public override event Action<TutorialStage> Completed;

    protected override void OnEnter()
    {
        FiringWeapon[] traps = _containersHandler.GetComponentsInChildren<FiringWeapon>();

        foreach (var firing in traps)
        {
            firing.Reloaded += OnFiringWeaponReloaded;
            ParticleSystem particleSystem = Instantiate(_template);
            ArrowIndicates arrowIndicates = Instantiate(_templateArrow, firing.transform);
            arrowIndicates.transform.localPosition = new Vector3(0, 4, 0);
            arrowIndicates.Init();
            particleSystem.transform.position = firing.transform.position.With(y: -2.1f);
            particleSystem.Play();
            _reloadTraps.Add(firing, particleSystem);
            _arrowIndicateTraps.Add(firing, arrowIndicates);
        }


        _taskActivable.TryActivate();
        _taskActivable.Finished += OnTaskFinished;
        _hintHandler.Add(_message);
        _taskBarHandler.AddTask(_taskActivable);
    }

    private void OnTaskFinished(ITaskable taskable)
    {
        taskable.Finished -= OnTaskFinished;
        Completed?.Invoke(this);
    }

    protected override void OnExit()
    {
       
    }

    private void OnFiringWeaponReloaded(FiringWeapon firingWeapon)
    {
        firingWeapon.Reloaded -= OnFiringWeaponReloaded;
        Destroy(_arrowIndicateTraps[firingWeapon].gameObject);
        _arrowIndicateTraps.Remove(firingWeapon);
        _reloadTraps[firingWeapon].Stop();
        _reloadTraps.Remove(firingWeapon);
    }

    //private IEnumerator TaskBarShowDelay()
    //{
    //    yield return new WaitUntil(() => _hintHandler.IsAvailable == false);

    //    _taskBarHandler.AddTask(_taskActivable);
    //}
}
