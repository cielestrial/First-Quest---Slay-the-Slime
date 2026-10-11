using System.Collections;
using UnityEngine;

namespace States.Knight
{
  public class MoveState : State
  {
    public static new readonly string Name = "Move";
    public static new readonly int StateHash = Animator.StringToHash(Name);
    public MoveState(UnitController unit, Animator animator, Transform transform) : base(unit, animator, transform) { }


    public override void OnEnter()
    {
      // MyRangeManager.ShowRanges(MyGridManager, SelectedUnit.transform.position, SelectedUnit.SkillRange[SelectedUnit.MyStateManager.CurrentState.Name]);
    }

    public override IEnumerator OnCoroutine()
    {
      yield return null;
    }

    public override void OnExit()
    {
      myAnimator.SetBool(StateHash, false);
    }

    public override bool GuardCondition()
    {
      return true;
    }
  }
}