using UnityEngine;

public interface ITaskActivable : ITaskable
{
    public bool TryActivate();
}
