using System;
using System.Collections.Generic;
using UnityEngine;

public class RaiseGarbage : TutorialStage
{
    [SerializeField] private ArrowIndicates _template;
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private TutorialStage _tutorialStage;

    private List<ArrowIndicates> _arrowIndicates = new List<ArrowIndicates>();
    private int _countResource;

    public override event Action<TutorialStage> Completed;

    private void OnEnable()
    {
        _resourceSpawner.ResourceSpawned += OnResourceSpawned;
    }

    private void OnDisable()
    {
        _resourceSpawner.ResourceSpawned -= OnResourceSpawned;
    }

    protected override void OnEnter()
    {

    }

    protected override void OnExit()
    {

    }

    private void OnResourceSpawned(Resource resource)
    {
        if (_tutorialStage.gameObject.activeSelf)
            return;

        resource.Picked += OnResourcePicked;
        ArrowIndicates arrowIndicates = Instantiate(_template, resource.transform);
        arrowIndicates.transform.localPosition = new Vector3(0, 1.5f, 0);
        arrowIndicates.Init();
        _arrowIndicates.Add(arrowIndicates);
        _countResource++;
    }

    private void OnResourcePicked(Resource garbage)
    {
        garbage.Picked -= OnResourcePicked;
        _countResource--;

        if (_countResource == 0)
        {
            foreach (var arrow in _arrowIndicates)
                Destroy(arrow.gameObject);

            Completed?.Invoke(this);
        }
    }
}
