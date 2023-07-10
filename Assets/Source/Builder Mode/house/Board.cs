using UnityEngine;

public class Board : MonoBehaviour, IBreakable
{
    private Rigidbody[] _pieces;

    private float _currentSafetyMargin;
    private float _maxSafetyMargin = 10;
    private Vector3[] _piecesDefaultPosition;
    private Quaternion[] _piecesDefaultRotation;

    public bool IsBroken => _currentSafetyMargin == 0;
    public bool IsRepaired => _currentSafetyMargin >= _maxSafetyMargin;

    private void Awake()
    {
        _currentSafetyMargin = _maxSafetyMargin;
        _pieces = GetComponentsInChildren<Rigidbody>();
        _piecesDefaultPosition = new Vector3[_pieces.Length];
        _piecesDefaultRotation = new Quaternion[_pieces.Length];

        for (int i = 0; i < _pieces.Length; i++)
        {
            _piecesDefaultPosition[i] = _pieces[i].transform.position;
            _piecesDefaultRotation[i] = _pieces[i].transform.rotation;
        }
    }

    public void Break(int damage)
    {
        if (IsBroken)
            return;

        _currentSafetyMargin -= damage;

        if (IsBroken)
            foreach (var piece in _pieces)
                piece.isKinematic = false;
    }

    public void SetKinematic(bool state)
    {
        foreach (var piece in _pieces)
            piece.isKinematic = state;
    }

    public void Repair()
    {
        if (IsRepaired)
            return;

        _currentSafetyMargin += Time.deltaTime / 3;

        for (int i = 0; i < _pieces.Length; i++)
        {
            _pieces[i].transform.position = Vector3.Lerp(_pieces[i].transform.position, _piecesDefaultPosition[i], _currentSafetyMargin/_maxSafetyMargin);
            _pieces[i].transform.rotation = Quaternion.Lerp(_pieces[i].transform.rotation, _piecesDefaultRotation[i], _currentSafetyMargin / _maxSafetyMargin);
        }

        if (IsRepaired)
        {
            _currentSafetyMargin = _maxSafetyMargin;
        }
    }
}
