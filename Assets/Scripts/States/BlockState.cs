using System.Collections;
using UnityEngine;

public class BlockState : NoClickState
{
  public new const string Name = "Block";
  public static new readonly int Hash = Animator.StringToHash(Name);
  public BlockState(UnitController unit) : base(unit)
  {
    SetName(Name);
    SetHash(Hash);
  }

  public override void OnEnter()
  {
    base.OnEnter();
    animator.SetBool(IdleState.Hash, true);
    animator.SetBool(base.Hash, true);
  }

  public override IEnumerator OnCoroutine()
  {
    return null;
  }

  public override IEnumerator OnExit()
  {
    animator.SetBool(base.Hash, false);
    animator.SetBool(IdleState.Hash, false);
    return base.OnExit();
  }
}