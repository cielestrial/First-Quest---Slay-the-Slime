using System.Collections;
using UnityEngine;

public class IdleState : State
{
  public static new readonly string Name = "Idle";
  public static new readonly int StateHash = Animator.StringToHash(Name);
  public IdleState(UnitController unit, Animator animator, Transform transform)
    : base(unit, animator, transform) { }


  public override void OnEnter()
  {
    myAnimator.SetBool(StateHash, true);
  }

  public override IEnumerator OnCoroutine()
  {
    yield return null;
  }

  public override void OnExit()
  {
    myAnimator.SetBool(StateHash, false);
  }

  public override bool GuardCondition() => true;

}
