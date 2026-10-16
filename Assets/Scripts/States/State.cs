using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
  public string Name { get; protected set; }
  public int Hash { get; protected set; }
  protected readonly UnitController unit;
  protected readonly Animator animator;
  protected readonly Transform transform;
  protected readonly GridManager gridManager;
  protected readonly TargetingSystem targetingSystem;
  protected readonly EventBus eventBus;
  protected bool abortCoroutine;
  protected State(UnitController unit)
  {
    this.unit = unit;
    animator = unit.MyAnimator;
    transform = unit.transform;
    gridManager = unit.MyGridManager;
    targetingSystem = unit.MyTargetingSystem;
    eventBus = unit.MyEventBus;
  }
  public void SetName(string name) { Name = name; }
  public void SetHash(int hash) { Hash = hash; }
  public virtual void OnEnter()
  {
    abortCoroutine = false;
  }
  public abstract IEnumerator OnCoroutine();
  public virtual IEnumerator OnExit()
  {
    abortCoroutine = true;
    while (IsAnimationPlaying(Name))
      yield return null;
  }
  public virtual bool CanEnter()
    => unit.SkillTree.ContainsKey(Name) && unit.SufficientResources(unit.SkillCost[Name]);
  public virtual bool CanExit() => true;

  public bool IsAnimationPlaying(string animName) =>
    animator.GetCurrentAnimatorStateInfo(0).IsName(animName) &&
    animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
}

public abstract class NoClickState : State
{
  protected NoClickState(UnitController unit) : base(unit) { }
}

// public abstract class FreeClickState : State
// {
//   protected FreeClickState(UnitController unit) : base(unit) { }
//   public override IEnumerator OnCoroutine() { yield return RangeSelect(); }
//   protected abstract IEnumerator RangeSelect();
//   protected abstract IEnumerator Action(List<Vector2Int> path);
// }

public abstract class EmptyClickState : State
{
  protected EmptyClickState(UnitController unit) : base(unit) { }
  public override IEnumerator OnCoroutine() { yield return RangeSelect(); }
  protected abstract IEnumerator RangeSelect();
  protected abstract IEnumerator Action(List<Vector2Int> path);
}

public abstract class EnemyClickState : State
{
  protected EnemyClickState(UnitController unit) : base(unit) { }
  public override IEnumerator OnCoroutine() { yield return RangeSelect(); }
  protected abstract IEnumerator RangeSelect();
  protected abstract IEnumerator Action(Vector3 rangePos, List<Vector2Int> victims);
  protected IEnumerator RotateIntoAction(Vector3 rangePos, int moveAnimHash)
  {
    Vector3 directionVector = rangePos - transform.position;
    directionVector.y = 0f;
    // avoid Quaternion.LookRotation error on a zero vector
    if (directionVector.sqrMagnitude > 0.0001f)
    {
      Quaternion targetRotation = Quaternion.LookRotation(directionVector);
      animator.SetBool(moveAnimHash, true);
      while (transform.rotation != targetRotation)
      {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, unit.TurnSpeed * Time.deltaTime);
        if (transform.rotation != targetRotation)
          yield return null;
      }
    }
    animator.SetTrigger(Hash);
    animator.SetBool(moveAnimHash, false);
  }
}