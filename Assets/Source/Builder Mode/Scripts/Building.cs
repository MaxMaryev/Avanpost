using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IContainerable, ISafetyMarginable, IDamageable, ITargetable
{
    [SerializeField] private List<BoxCollider> _solidColliders;
    [SerializeField] private List<Renderer> _renderers;
    [SerializeField] private ParticleSystem _dustEffect;
    [SerializeField] private List<BoxCollider> _breakColliders;
    [SerializeField] private AudioClip _audioClipPlaced;
    [SerializeField] private AudioClip _audioClipDestroy;

    private BuildingRemoving _buildingRemoving;

    public AudioClip ClipPlaced => _audioClipPlaced;
    public AudioClip ClipDestroy => _audioClipDestroy;
    public BuildingClass Class { get; private set; }
    public BuildingName Name { get; private set; }
    public ConstructorCell Cell { get; private set; }
    public BuildingsSaver BuildingsSaver { get; private set; }
    public float MaxSafetyMargin { get; protected set; }
    public float CurrentSafetyMargin { get; private set; }
    public bool IsDestroying { get; private set; }
    public string Id { get; private set; }
    public BuildingData BuildingData { get; private set; }

    public bool IsAlive => CurrentSafetyMargin > 0;

    public Vector3 Position => transform.position;

    public event Action<float, float, Building> SafetyMarginChanged;
    public event Action Placed;
    public event Action<Building> Destroying;

    public void Init(BuildingName identifier, BuildingClass buildingClass, ConstructorCell cell, float maxSafetyMargin,
        BuildingRemoving buildingRemoving, float? currentSafetyMargin = null)
    {
        Name = identifier;
        Class = buildingClass;
        Cell = cell;
        MaxSafetyMargin = maxSafetyMargin;
        _buildingRemoving = buildingRemoving;
        CurrentSafetyMargin = currentSafetyMargin ?? maxSafetyMargin;
    }

    public void InitBuildingsSaver(BuildingsSaver buildingsSaver) => BuildingsSaver = buildingsSaver;

    public void InitBuildingData(string value)
    {
        Id = value;
        BuildingData = new BuildingData(Id);
    }

    public void Place(Transform container)
    {
        if (_dustEffect != null)
            _dustEffect.Play();

        transform.SetParent(container);

        if (BuildingsSaver != null)
            BuildingsSaver.SaveOnQuitGame(this);

        Placed?.Invoke();
    }

    public void Repair()
    {
        if (_dustEffect != null)
            _dustEffect.Play();

        CurrentSafetyMargin = MaxSafetyMargin;
        SafetyMarginChanged?.Invoke(MaxSafetyMargin, CurrentSafetyMargin, this);
    }

    public IEnumerator StartDestroying(bool isFast = false)
    {
        IsDestroying = true;
        Destroying?.Invoke(this);
        OnStartDestroying();
        _buildingRemoving.Remove(this);

        if (isFast == false)
        {
            if (_dustEffect != null)
            {
                foreach (var renderer in _renderers)
                    renderer.enabled = false;

                if (_solidColliders.Count > 0)
                    foreach (var solidCollider in _solidColliders)
                        Destroy(solidCollider);

                foreach (var collider in _breakColliders)
                    collider.transform.position = Vector3.zero;

                _dustEffect.Play();
            }

            yield return new WaitForSeconds(_dustEffect.main.duration);
        }

        Destroy(gameObject);
        yield return null;
    }

    protected void SetDustEffectPosition(Vector3 position) => _dustEffect.transform.position = position;

    protected virtual void OnStartDestroying() { }

    public void TakeDamage(float damage)
    {
        if (CurrentSafetyMargin <= 0)
            return;

        CurrentSafetyMargin -= damage;
        SafetyMarginChanged?.Invoke(MaxSafetyMargin, CurrentSafetyMargin, this);

        if (CurrentSafetyMargin <= 0)
        {
            if (BuildingsSaver != null)
                BuildingsSaver.DeleteFromSaveList(this);

            StartCoroutine(StartDestroying());
        }
    }
}