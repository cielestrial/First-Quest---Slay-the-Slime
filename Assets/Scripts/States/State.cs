using System.Collections;
using UnityEngine;


public abstract class State
{
  public readonly string Name;
  public readonly int StateHash;
  protected readonly UnitController self;
  protected readonly Animator myAnimator;
  protected readonly Transform myTransform;

  protected State(UnitController unit, Animator animator, Transform transform)
  {
    self = unit;
    myAnimator = animator;
    myTransform = transform;
  }

  public abstract void OnEnter();
  public abstract IEnumerator OnCoroutine();
  public abstract void OnExit();
  public abstract bool GuardCondition();
}

public delegate bool Guard();

public abstract class StateManager
{
  public State CurrentState { get; protected set; }

  protected StateManager(State initialState)
  {
    CurrentState = initialState;
    CurrentState.OnEnter();
  }

  public bool IsIdle() => CurrentState.StateHash == IdleState.StateHash ||
    CurrentState.StateHash == BlockState.StateHash;


  public bool Transition(State to)
  {
    if (CurrentState.StateHash == to.StateHash) return false;
    if (!to.GuardCondition()) return false;

    CurrentState.OnExit();
    CurrentState = to;
    CurrentState.OnEnter();
    return true;
  }
}