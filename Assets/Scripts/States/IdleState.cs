using System.Collections;
using UnityEngine;

public class IdleState : State
{
  public new const string Name = "Idle";
  public static new readonly int Hash = Animator.StringToHash(Name);
  public IdleState(UnitController unit) : base(unit)
  {
    SetName(Name);
    SetHash(Hash);
  }

  public override void OnEnter()
  {
    base.OnEnter();
    animator.SetBool(base.Hash, true);
  }

  public override IEnumerator OnCoroutine()
  {
    return null;
  }

  public override IEnumerator OnExit()
  {
    animator.SetBool(base.Hash, false);
    return base.OnExit();
  }

  public override bool CanEnter() => true;
}
