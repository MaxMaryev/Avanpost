using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> currentTransitions = new List<Transition>();
    private List<Transition> emptyTransitions = new List<Transition>(0);
    private BaseState _currentState;

    public BaseState CurrentState => _currentState;

    public void ChangeState(BaseState newState)
    {
        if (newState == _currentState)
            return;

        _currentState?.Exit();
        _currentState = newState;

        if (_transitions.ContainsKey(_currentState.GetType()))
            currentTransitions = _transitions[_currentState.GetType()];
        else
            currentTransitions = emptyTransitions;

        newState.Enter();
    }

    public void AddTransition(BaseState from, BaseState to, Func<bool> predicate)
    {
        if (_transitions.ContainsKey(from.GetType()))
        {
            _transitions[from.GetType()].Add(new Transition(from, to, predicate));
        }
        else
        {
            _transitions[from.GetType()] = new List<Transition>();
            _transitions[from.GetType()].Add(new Transition(from, to, predicate));
        }
    }

    public void Tick()
    {
        var transition = GetTransition();

        if (transition != null)
            ChangeState(transition.To);

        _currentState?.Tick();
    }

    private Transition GetTransition()
    {
        foreach (var transition in currentTransitions)
            if (transition.predicate())
                return transition;

        return null;
    }
}
