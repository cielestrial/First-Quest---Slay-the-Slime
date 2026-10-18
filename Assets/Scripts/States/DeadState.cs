using System.Collections;
using UnityEngine;

public class DeadState : State
{
  public new const string Name = "Dead";
  public static new readonly int Hash = Animator.StringToHash(Name);
  public DeadState(UnitController unit) : base(unit)
  {
    SetName(Name);
    SetHash(Hash);
  }

  public override void OnEnter()
  {
    base.OnEnter();
    animator.SetBool(base.Hash, true);
    eventBus.MyTurnEvent.AddGarbage(unit);
  }

  public override IEnumerator OnCoroutine()
  {
    // First remove traces from field
    gridManager.RemoveFromDetectionGrid(unit.GridPosition);
    // remove from UI
    eventBus.MyUIEvent.RemoveUnit(unit);

    while (IsAnimationPlaying(base.Name))
      yield return null;

    eventBus.MyBattleEvent.UnitDeath(unit);
  }

  public override IEnumerator OnExit()
  {
    animator.SetBool(base.Hash, false);
    return base.OnExit();
  }

  public override bool CanEnter() => unit.CurrentHP <= 0;
  public override bool CanExit() => unit.CurrentHP > 0;


}
