using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private BuildingInstallation _buildingInstallation;

    private void OnEnable()
    {
        //_building.Placed += OnBuildingPlaced;
        //_building.Destroying += OnBuildingDestroying;
        _buildingInstallation.Done += OnDone;
    }

    private void OnDisable()
    {
        //_building.Placed -= OnBuildingPlaced;
        //_building.Destroying -= OnBuildingDestroying;
        _buildingInstallation.Done -= OnDone;

    }

    private void OnDone(Building building)
    {
        PlayClip(building.ClipPlaced);
        building.Destroying += OnBuildingDestroying;
    }

    private void OnBuildingDestroying(Building building)
    {
        building.Destroying -= OnBuildingDestroying;
        PlayClip(building.ClipDestroy);
    }

    private void PlayClip(AudioClip audioClip)
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
