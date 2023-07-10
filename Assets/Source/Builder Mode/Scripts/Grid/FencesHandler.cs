using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class FencesHandler : MonoBehaviour
{
    [SerializeField] private List<FenceChain> _fenceChains;
    [SerializeField] private BuildingInstallation _buildingInstallation;
    [SerializeField] private GameModeHandler _gameModeHandler;
    [SerializeField] private BuildingsPanel _buildingsPanel;

    private Coroutine _splitingCoroutine;
    private Queue<Fence> _fencesQueue = new Queue<Fence>();

    public IReadOnlyList<FenceChain> FenceChains => _fenceChains;
    public bool IsNeedEdjesUpdate => _gameModeHandler.CurrentGameMode == GameModes.ConstructorMode;

    private void OnEnable()
    {
        _buildingsPanel.Opened += OnBuildingsPanelOpened;
        _buildingInstallation.Done += OnBuildingInstallationDone;
    }

    private void OnDisable()
    {
        _buildingsPanel.Opened -= OnBuildingsPanelOpened;
        _buildingInstallation.Done -= OnBuildingInstallationDone;
    }

    public FenceChain CreateNewChain(int? chainId = null, params Fence[] fences)
    {
        FenceChain newFenceChain = new FenceChain(this, fences, chainId);
        _fenceChains.Add(newFenceChain);

        for (int i = 0; i < fences.Length; i++)
            fences[i].InitChainId(newFenceChain.Id);

        return newFenceChain;
    }

    public void CombineTwoChains(int mainChainID, int connectingChainID, int connectingFenceOrderNumber)
    {
        FenceChain mainChain = GetChain(mainChainID);
        FenceChain connectingChain = GetChain(connectingChainID);

        OrderCombinedChains(mainChain, connectingChain, connectingFenceOrderNumber);

        FenceChain newChain = new FenceChain(this ,mainChain.Fences);
        newChain.AddRange(connectingChain.Fences);
        Fence[] fences = newChain.Fences.ToArray();
        CreateNewChain(fences: fences);

        DeleteChain(mainChain);
        DeleteChain(connectingChain);
    }

    public void SplitChain(Fence fence)
    {
        _fencesQueue.Enqueue(fence);

        if (_splitingCoroutine == null)
            _splitingCoroutine = StartCoroutine(Spliting());

        IEnumerator Spliting()
        {
            while (_fencesQueue.Count > 0)
            {
                Fence fence = _fencesQueue.Dequeue();
                FenceChain originalChain = fence.FenceChain;
                List<Fence> fencesOfNewChain1 = new List<Fence>();
                List<Fence> fencesOfNewChain2 = new List<Fence>();

                for (int i = 0; i < fence.OrderNumber; i++)
                    fencesOfNewChain1.Add(originalChain.Fences[i]);

                for (int i = fence.OrderNumber + 1; i < originalChain.Fences.Count; i++)
                    fencesOfNewChain2.Add(originalChain.Fences[i]);

                DeleteChain(originalChain);
                FenceChain fenceChain1 = CreateNewChain(fences: fencesOfNewChain1.ToArray());
                FenceChain fenceChain2 = CreateNewChain(fences: fencesOfNewChain2.ToArray());

                fenceChain1.Update(IsNeedEdjesUpdate);
                fenceChain2.Update(IsNeedEdjesUpdate);

                if (IsNeedEdjesUpdate)
                    fenceChain2.Fences[0].ShowModel(FenceModel.Column);

                yield return null;
            }

            _splitingCoroutine = null;
        }
    }

    public void OrderCombinedChains(FenceChain mainChain, FenceChain connectingChain, int connectingFenceOrderNumber)
    {
        if (connectingFenceOrderNumber == 0)
            for (int i = 0; i < connectingChain.Fences.Count; i++)
                connectingChain.Fences[i].InitOrderNumber(mainChain.Fences.Count - 1 + i);
        else
            for (int i = 0; i < connectingChain.Fences.Count; i++)
                connectingChain.Fences[i].InitOrderNumber(mainChain.Fences.Count + connectingFenceOrderNumber - i);
    }

    public void DeleteChain(FenceChain fenceChain) => _fenceChains.Remove(fenceChain);

    public void OnBuildingsPanelOpened()
    {
        foreach (var fenceChain in _fenceChains)
            fenceChain.Update(isNeedEdjesUpdate: true);
    }

    public FenceChain GetChain(int chainId)
    {
        for (int i = 0; i < _fenceChains.Count; i++)
            if (_fenceChains[i].Id == chainId)
                return _fenceChains[i];

        return null;
    }

    private void OnBuildingInstallationDone(Building building)
    {
        if (building is Fence fence)
            fence.InitFencesHandler(this);
    }
}

[Serializable]
public class FenceChain
{
    [SerializeField] private List<Fence> _fences = new List<Fence>();

    private FencesHandler _fencesHandler;

    public FenceChain(FencesHandler fencesHandler, IReadOnlyList<Fence> fences = null, int? id = null)
    {
        _fences.AddRange(fences);
        _fencesHandler = fencesHandler;
        Id = id ?? UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    }

    public int Id { get; private set; }
    public IReadOnlyList<Fence> Fences => _fences;

    public void Add(Fence fence)
    {
        fence.InitChainId(Id);
        _fences.Add(fence);
    }

    public void AddRange(IReadOnlyList<Fence> fence)
    {
        for (int i = 0; i < fence.Count; i++)
            _fences.Add(fence[i]);
    }

    public void Remove(Fence fence)
    {
        _fences.Remove(fence);
        Update(_fencesHandler.IsNeedEdjesUpdate);
    }

    public void InitNeighbors()
    {
        if (_fences.Count < 2)
            return;

        for (int i = 0; i < _fences.Count; i++)
        {
            if (i == 0)
            {
                _fences[i].AddNeighbor(_fences[i + 1]);
            }
            else if (i == _fences.Count - 1)
            {
                _fences[i].AddNeighbor(_fences[i - 1]);
            }
            else
            {
                _fences[i].AddNeighbor(_fences[i - 1]);
                _fences[i].AddNeighbor(_fences[i + 1]);
            }
        }
    }

    public void Update(bool isNeedEdjesUpdate)
    {
        _fences = _fences.OrderBy(x => x.OrderNumber).ToList();

        for (int i = 0; i < _fences.Count; i++)
        {
            if (_fences[i] == null)
                continue;

            if (_fences[i].FenceChain != this)
                _fences[i].InitFenceChain(this);
#if UNITY_EDITOR
            _fences[i].transform.SetSiblingIndex(i);
#endif

            _fences[i].InitOrderNumber(i);

            if (isNeedEdjesUpdate)
            {
                if (i > 0)
                    _fences[i].AlignWithNeighbor(_fences[i - 1]);

                ShowEdjes(i);
            }
        }

        void ShowEdjes(int index)
        {
            if (_fences.Count < 3)
                _fences[index].ShowModel(FenceModel.Column);

            if (index == 0 || index == _fences.Count - 1)
            {
                if(index == 0)
                    _fences[index].ShowModel(FenceModel.Column);

                _fences[index].ShowEdje(true);
            }
            else
            {
                _fences[index].ShowEdje(false);
            }
        }
    }

    public void ReverseOrderNumbers()
    {
        for (int i = 0; i < _fences.Count; i++)
            _fences[i].InitOrderNumber(_fences.Count - 1 - i);
    }
}

