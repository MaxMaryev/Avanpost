using System;
using UnityEngine;

public class Transition
{
    public BaseState From;
    public BaseState To;
    public Func<bool> predicate;

    public Transition(BaseState From, BaseState To, Func<bool> predicate)
    {
        this.From = From;
        this.To = To;
        this.predicate = predicate;
    }
}
