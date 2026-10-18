using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States.Slime
{
  public class MoveState : EmptyClickState
  {
    public new const string Name = "Move";
    public static new readonly int Hash = Animator.StringToHash(Name);

    public MoveState(UnitController unit) : base(unit)
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
      Vector2Int start, end;
      List<Vector2Int> path = new(0);

      while (path.Count == 0)
      {
        if (abortCoroutine)
        {
          abortCoroutine = false;
          break;
        }
        else if (Mouse.current.leftButton.wasPressedThisFrame)
        {
          Vector3? targetLocation = targetingSystem.GetClickLocation();
          if (targetLocation != null && targetingSystem.IsClickWithinRange((Vector3)targetLocation))
          {
            start = GridManager.ConvertWorldPosToGrid(transform.position);
            end = GridManager.ConvertWorldPosToGrid((Vector3)targetLocation);
            path = gridManager.FindMovePath(start, end, true);
          }
        }
        yield return null;
      }

      eventBus.MyRangeEvent.HideRange();
      // Path includes start position
      if (path.Count > 1)
        yield return Action(path);
    }

    protected override IEnumerator Action(List<Vector2Int> path)
    {
      Vector3 subTargetLocation;
      Quaternion targetRotation;
      Vector3 directionVector;

      eventBus.MyUIEvent.HideUI();
      animator.SetBool(base.Hash, true);
      // Path includes start position
      for (int pathIndex = 1, pathSize = path.Count; pathIndex < pathSize; ++pathIndex)
      {
        subTargetLocation = gridManager.LocationGrid[path[pathIndex].x, path[pathIndex].y];
        while (transform.position != subTargetLocation)
        {
          // Angle
          directionVector = subTargetLocation - transform.position;
          directionVector.y = 0f;
          // avoid Quaternion.LookRotation error on a zero vector
          if (directionVector.sqrMagnitude > 0.0001f)
          {
            targetRotation = Quaternion.LookRotation(directionVector);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, unit.TurnSpeed * Time.deltaTime);
          }
          // Movement
          transform.position = Vector3.MoveTowards(transform.position, subTargetLocation, unit.MoveSpeed * Time.deltaTime);
          yield return null;
        }

      }

      animator.SetBool(base.Hash, false);
      yield return null;
      gridManager.MoveInDetectionGrid(path[0], path[^1], unit);
      unit.ConsumeResources(unit.Actives[base.Name].Cost);
      // Action chosen
      yield return null;
      eventBus.MyTurnEvent.ActionTaken();
      yield return null;
    }

    public override IEnumerator OnExit()
    {
      animator.SetBool(base.Hash, false);
      return base.OnExit();
    }

    public override bool CanExit()
    {
      return !animator.GetBool(base.Hash);
    }
  }
}