using UnityEngine;

public abstract class BaseState
{
    protected Enemy Enemy { get; private set; }
    protected EnemyAnimationsController AnimationsController { get; private set; }

    public BaseState(Enemy enemy, EnemyAnimationsController animationsController)
    {
        Enemy = enemy;
        AnimationsController = animationsController;
    }

    public virtual void Enter() {
    }
    public virtual void Tick() { }
    public virtual void Exit() {
    }
}
