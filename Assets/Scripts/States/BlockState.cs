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

  public override IEnumerator OnEnter()
  {
    animator.SetBool(IdleState.Hash, true);
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
    animator.SetBool(IdleState.Hash, false);
    yield return base.OnExit();
  }
}