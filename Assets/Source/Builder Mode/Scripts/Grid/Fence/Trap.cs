using UnityEngine;

public class Trap : Building
{
    [SerializeField] private Mine _mine;

    public Mine Mine => _mine;
}
