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

    public override void OnEnter()
    {
      base.OnEnter();
      eventBus.MyRangeEvent.ShowRange(transform.position, unit.Actives[base.Name].Range);
    }

    protected override IEnumerator RangeSelect()
    {
      Vector2Int gridPos;
      List<Vector2Int> targetLocations = new(0);
      Vector3 rangePos = Vector3.zero;

      while (targetLocations.Count == 0)
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
              targetLocations = RangeManager.GetRangeAoE(rangeTransform.tag, gridPos, unit.Actives[base.Name].Range);
              Debug.Log($"before filter: {targetLocations.Count}");
              // aoe = gridManager.FilterEnemies(aoe);
              Debug.Log($"after filter: {gridManager.FilterEnemies(targetLocations).Count}");
            }
          }
        }
        yield return null;
      }

      eventBus.MyRangeEvent.HideRange();
      if (targetLocations.Count > 0)
        yield return Action(rangePos, targetLocations);
    }

    protected override IEnumerator Action(Vector3 rangePos, List<Vector2Int> targetLocations)
    {
      eventBus.MyUIEvent.HideUI();
      // Turn towards selected range
      yield return RotateIntoAction(rangePos, MoveState.Hash);
      // Trigger damage via new Battle Event
      eventBus.MyBattleEvent.DamageDealt(unit, unit.Actives[base.Name].AtkMod, targetLocations);
      yield return null;
      unit.ConsumeResources(unit.Actives[base.Name].Cost);
      yield return null;
      // Action chosen
      eventBus.MyTurnEvent.ActionTaken();
      yield return null;
    }

    public override IEnumerator OnExit()
    {
      while (IsAnimationPlaying(MoveState.Name))
        yield return null;
      base.OnExit();
    }
  }
}