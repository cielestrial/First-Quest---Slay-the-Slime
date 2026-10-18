using System.Collections;
using UnityEngine;

public abstract class StateManager
{
  public State CurrentState { get; protected set; }
  protected readonly UnitController unit;
  protected readonly Animator animator;
  public static readonly int SpeedHash = Animator.StringToHash("Speed");

  protected StateManager(State initialState, UnitController unit)
  {
    CurrentState = initialState;
    this.unit = unit;
    animator = unit.MyAnimator;
    SetAnimSpeed();
    CurrentState.OnEnter();
  }

  private void SetAnimSpeed()
  {
    if (animator.GetFloat(SpeedHash) != TurnManager.GameSpeed)
      animator.SetFloat(SpeedHash, TurnManager.GameSpeed);
  }

  public bool IsInBlockState() => animator.GetBool(BlockState.Hash);
  public bool IsInIdleOrBlockState() => animator.GetBool(IdleState.Hash);
  // CurrentState.Hash == IdleState.Hash || CurrentState.Hash == BlockState.Hash;

  public IEnumerator TransitionState(State to, bool CoroutineAutoStart = false)
  {
    if (CurrentState.Hash != to.Hash && CurrentState.CanExit() && to.CanEnter())
    {
      yield return CurrentState.OnExit();
      // yield return null;
      CurrentState = to;
      CurrentState.OnEnter();
      if (CoroutineAutoStart)
        yield return CurrentState.OnCoroutine();
    }
  }
}