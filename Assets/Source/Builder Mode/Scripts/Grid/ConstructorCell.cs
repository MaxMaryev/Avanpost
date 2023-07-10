using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConstructorCell : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Color _yellow;
    [SerializeField] private Color _white;
    [SerializeField] private Color _red;
    [SerializeField] private Sprite _greenCell;
    [SerializeField] private Sprite _frameCell;

    public event Action PointerUped;
    public event Action<Vector3, Vector2Int> PointerEntered;
    public event Action<Vector3, Vector2Int> PointerDowned;

    private List<ConstructorCell> _neighbors = new List<ConstructorCell>();
    private bool _hasDowned;

    [field: SerializeField] public Image Icon { get; private set; }
    [field: SerializeField] public Vector2Int Index { get; private set; }
    public Building Building { get; private set; }
    public List<ConstructorCell> Neighbors => _neighbors;

    public void InitNeighbors(IReadOnlyList<ConstructorCell> neighbors) => _neighbors.AddRange(neighbors);

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Building == null)
            ShowFill();

        PointerEntered?.Invoke(transform.position, Index);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUped?.Invoke();
        _hasDowned = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Building == null)
        {
            if (Icon.sprite != _greenCell)
                Icon.sprite = _greenCell;

            Icon.color = _yellow;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_hasDowned == true)
            return;

        _hasDowned = true;
        PointerDowned?.Invoke(transform.position, Index);
    }

    public void Fill(Building building)
    {
        Building = building;
        ShowFill();
    }

    private void Color(Sprite sprite, Color color)
    {
        if (Icon.sprite != sprite)
            Icon.sprite = sprite;

        Icon.color = color;
    }

    public void ShowFill() => Color(_frameCell, _white);

    public void ShowBlock() => Color(_frameCell, _red);

    public void ShowEmpty()
    {
        if (Icon.color != _yellow && Building == null)
            Color(_greenCell, _yellow);
    }

    public void SetIndex(Vector2Int index) => Index = index;

    public void ClearCell()
    {
        Building = null;
        ShowEmpty();
    }
}
