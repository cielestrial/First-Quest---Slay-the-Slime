using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitController : MonoBehaviour
{
  public GridManager MyGridManager;
  public TargetingSystem MyTargetingSystem;
  public EventSystem MyEventSystem;
  public readonly int MoveSpeed = Mathf.RoundToInt(5 * TurnManager.GameSpeed);
  public readonly int TurnSpeed = Mathf.RoundToInt(315 * 1.75f * TurnManager.GameSpeed);
  public UnitSelect UnitSelectButton { get; protected set; }
  public int CurrentLevel = 1;
  public const int MaxLevel = 99;
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
  public Animator MyAnimator { get; protected set; }
  public string UnitName { get; protected set; }

  public enum ActionType
  {
    Idle = 0,
    NoClick = 1,
    FreeClick = 2,
    EmptyClick = 3,
    EnemyClick = 4
  }

  public readonly struct SkillResourceCost
  {
    public readonly int AP;
    public readonly int MP;

    public SkillResourceCost(int AP)
    {
      this.AP = AP;
      MP = 0;
    }
    public SkillResourceCost(int AP, int MP)
    {
      this.AP = AP;
      this.MP = MP;
    }
  }

  protected StateManager MyStateManager;

  public Dictionary<string, State> States { get; protected set; } = new(15);
  public Dictionary<string, int> SkillTree { get; protected set; } = new(15);
  public Dictionary<string, string> SkillDesc { get; protected set; } = new(15);
  public Dictionary<string, SkillResourceCost> SkillCost { get; protected set; } = new(15);
  public Dictionary<string, ActionType> SkillActionType { get; protected set; } = new(15);
  public Dictionary<string, int> SkillTreeUnlock { get; protected set; } = new(15);
  public Dictionary<string, int> UpgradeTree { get; protected set; } = new(15);
  public Dictionary<string, Vector2Int> SkillRange { get; protected set; } = new(15);

  public AwakeningSkills Awakening { get; protected set; }

  public virtual int GetCurrentAP() => CurrentAP;
  public virtual int GetTotalAP() => TotalAP;
  public abstract void ResetCurrentAP();
  public abstract void IncreaseTotalAP(int byValue);
  public abstract void ConsumeResources(SkillResourceCost cost);
  public abstract bool SufficientResources(SkillResourceCost cost);
  protected int ConsumeResource(int current, int cost)
    => current - cost < 0 ? 0 : current - cost;

  protected int AddResource(int current, int total, int boon)
    => (current + boon > total) ? total : current + boon;

  protected bool SufficientResource(int current, int cost)
  => current - cost >= 0;

  public static Vector2Int GenerateSquareRange(int factor) => new(factor * 2, factor);

  public IEnumerator PerformAction(string actionName)
  {
    yield return MyStateManager.TransitionState(States[actionName]);
  }

  public bool IsInIdleOrBlockState() => MyStateManager.IsInIdleOrBlockState();

  public IEnumerator CurrentStateCoroutine()
  {
    yield return MyStateManager.CurrentState.OnCoroutine();
  }

  public void SetUnitSelectButton(UnitSelect _button)
  {
    UnitSelectButton = _button;
  }
}
