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

  public override IEnumerator OnEnter()
  {
    animator.SetBool(base.Hash, true);
    yield return base.OnEnter();
  }

  public override IEnumerator OnCoroutine()
  {
    yield return null;
  }

  public override IEnumerator OnExit()
  {
    animator.SetBool(base.Hash, false);
    yield return base.OnExit();
  }

  public override bool CanEnter() => true;
}
