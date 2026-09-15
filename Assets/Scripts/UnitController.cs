using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnitController : MonoBehaviour
{
  [SerializeField] protected GridManager MyGridManager;
  [SerializeField] protected int MoveSpeed = 5;
  [SerializeField] protected int TurnSpeed = 10;
  public UnitSelect UnitSelectButton { get; private set; }
  public int CurrentLevel = 1;
  public int MaxLevel = 99;
  public int CurrentHP = 100;
  public int TotalHP = 100;
  public int CurrentMP = 10;
  public int TotalMP = 10;
  public int CurrentExp = 0;
  public int ExpToNextLevel = 2;
  public int CurrentSP = 1;
  public int TotalSP = 1;
  public int Attack = 10;
  public int Defense = 5;

  protected int CurrentAP = 2;
  protected int TotalAP = 2;
  protected Animator myAnimator;
  public string UnitName;
  // public bool CurrentTurn = false;
  public string CurrentAction = "Idle";
  public static readonly int MoveStateHash = Animator.StringToHash("Move");
  public enum ActionType
  {
    Idle = 0,
    NoClick = 1,
    FreeClick = 2,
    EmptyClick = 3,
    EnemyClick = 4
  }
  public ActionType CurrentActionType { get; private set; } = ActionType.Idle;

  public Dictionary<string, int> SkillTree { get; protected set; } = new(15);
  public Dictionary<string, string> SkillDesc { get; protected set; } = new(15);
  public Dictionary<string, ActionType> SkillActionType { get; protected set; } = new(15);
  public Dictionary<string, int> SkillTreeUnlock { get; protected set; } = new(15);
  public Dictionary<string, int> UpgradeTree { get; protected set; } = new(15);
  public Dictionary<string, Vector2Int> SkillRange { get; protected set; } = new(15);

  public AwakeningSkills Awakening { get; private set; }

  void Start()
  {
    myAnimator = GetComponent<Animator>();
  }

  public virtual int GetCurrentAP() => CurrentAP;
  public virtual int GetTotalAP() => TotalAP;
  public virtual void SetCurrentAP(int toValue) { }
  public virtual void ResetCurrentAP() { }
  public virtual void IncreaseTotalAP(int byValue) { }


  public void SetTurn()
  {
    Idle();
    TurnManager.Instance.SelectedUnit = this;
  }

  public static Vector2Int GenerateSquareRange(int factor) => new(factor * 2, factor);

  public virtual void PerformAction(string actionName) { }

  public void Idle()
  {
    CurrentActionType = ActionType.Idle;
    CurrentAction = "Idle";
    UIManager.Instance.ClearActionDescription();
  }
  public bool IsIdle() => CurrentActionType == ActionType.Idle;
  public void NoClick()
  {
    CurrentActionType = ActionType.NoClick;
  }
  public void FreeClick()
  {
    CurrentActionType = ActionType.FreeClick;
  }
  public void EmptyClick()
  {
    CurrentActionType = ActionType.EmptyClick;
  }
  public void EnemyClick()
  {
    CurrentActionType = ActionType.EnemyClick;
  }

  public void SetUnitSelectButton(UnitSelect _button)
  {
    UnitSelectButton = _button;
  }

  public IEnumerator Move(List<Vector2Int> path)
  {
    Vector3 subTargetLocation;
    Quaternion targetRotation;
    Vector3 directionVector;

    myAnimator.SetBool(MoveStateHash, true);
    for (int pathIndex = 0, pathSize = path.Count; pathIndex < pathSize; ++pathIndex)
    {
      subTargetLocation = MyGridManager.LocationGrid[path[pathIndex].x, path[pathIndex].y];
      while (transform.position != subTargetLocation)
      {
        // Angle
        directionVector = subTargetLocation - transform.position;
        directionVector.y = 0f; // ignore any vertical difference, stay upright — grid movement is flat
        if (directionVector.sqrMagnitude > 0.0001f) // avoid Quaternion.LookRotation error on a zero vector
        {
          targetRotation = Quaternion.LookRotation(directionVector);
          transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
        }
        // Movement
        transform.position = Vector3.MoveTowards(transform.position, subTargetLocation, MoveSpeed * Time.deltaTime);
        yield return null;
      }

    }
    myAnimator.SetBool(MoveStateHash, false);
  }
}
