using System.Collections.Generic;
using UnityEngine;

public interface IFriskableHandler
{
    public int NumbersOfAvailableFriskable { get; }
    public IReadOnlyList<IFriskable> Friskables { get; }
}
