using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States.Knight
{
  public class SlashState : EnemyClickState
  {
    public new const string Name = "Slash";
    public static new readonly int Hash = Animator.StringToHash(Name);

    public SlashState(UnitController unit) : base(unit)
    {
      SetName(Name);
      SetHash(Hash);
    }

    public override IEnumerator OnEnter()
    {
      eventSystem.MyRangeEvent.ShowRange(transform.position, unit.SkillRange[base.Name]);
      yield return base.OnEnter();
    }

    protected override IEnumerator RangeSelect()
    {
      Vector2Int gridPos;
      List<Vector2Int> aoe = new(0);
      Vector3 rangePos = Vector3.zero;

      while (aoe.Count == 0)
      {
        if (abortCoroutine)
        {
          abortCoroutine = false;
          break;
        }
        else if (Mouse.current.leftButton.wasPressedThisFrame)
        {
          Vector3? targetLocation = targetingSystem.GetClickLocation();
          if (targetLocation != null)
          {
            Transform rangeTransform = targetingSystem.GetTargetRange((Vector3)targetLocation);
            if (rangeTransform != null)
            {
              rangePos = rangeTransform.position;
              gridPos = GridManager.ConvertWorldPosToGrid(rangePos);
              aoe = RangeManager.GetRangeAoE(rangeTransform.tag, gridPos, unit.SkillRange[base.Name]);
              Debug.Log($"before filter: {aoe.Count}");
              // aoe = gridManager.FilterEnemies(aoe);
              Debug.Log($"after filter: {gridManager.FilterEnemies(aoe).Count}");
            }
          }
        }
        yield return null;
      }

      eventSystem.MyRangeEvent.HideRange();
      if (aoe.Count > 0)
        yield return Action(rangePos, aoe);
    }

    protected override IEnumerator Action(Vector3 rangePos, List<Vector2Int> victims)
    {
      // Turn towards selected range
      yield return RotateIntoAction(rangePos, MoveState.Hash);
      // Trigger damage via new Battle Event
      // eventSystem.BattleEvent.Damage(victims);
      yield return null;
      unit.ConsumeResources(unit.SkillCost[base.Name]);
      yield return null;
      // Action chosen
      eventSystem.MyTurnEvent.ActionTaken();
    }

    public override IEnumerator OnExit()
    {
      while (IsAnimationPlaying(MoveState.Name))
        yield return null;
      yield return base.OnExit();
    }
  }
}