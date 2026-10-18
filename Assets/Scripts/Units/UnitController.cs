using System.Collections;
using System.Collections.Generic;
using Grid.Data;
using Unit.Data;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class UnitController : MonoBehaviour, IFocusable
{
  public GridManager MyGridManager { get; private set; }
  public TargetingSystem MyTargetingSystem { get; private set; }
  public EventBus MyEventBus { get; private set; }
  public readonly int MoveSpeed = Mathf.RoundToInt(5 * TurnManager.GameSpeed);
  public readonly int TurnSpeed = Mathf.RoundToInt(315 * 1.75f * TurnManager.GameSpeed);
  public UnitSelect UnitSelectButton { get; protected set; }
  public Vector3 FocusPosition => transform.position;
  public Vector2Int GridPosition => GridManager.ConvertWorldPosToGrid(transform.position);
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
  public int Defence = 5;
  protected int CurrentAP = 2;
  protected int TotalAP = 2;
  public DetectionType DetectionType { get; protected set; }
  public Animator MyAnimator { get; protected set; }
  public string UnitName { get; protected set; }

  protected StateManager MyStateManager;

  public Dictionary<string, State> States { get; protected set; } = new(15);
  public Dictionary<string, Skill> SkillTree { get; protected set; } = new(15);
  public Dictionary<string, Skill> Actives { get; protected set; } = new(15);
  public Dictionary<string, Skill> Passives { get; protected set; } = new(15);


  public AwakeningSkills Awakening { get; protected set; }

  public virtual void SetUp(
    EventBus eventBus,
    GridManager gridManager,
    DetectionType detectionType,
    TargetingSystem targetingSystem
    )
  {
    MyEventBus = eventBus;
    MyGridManager = gridManager;
    DetectionType = detectionType;
    MyTargetingSystem = targetingSystem;
    MyAnimator = GetComponent<Animator>();
  }

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

  public void TakeDamage(int dmg)
  {
    ConsumeResource(CurrentHP, dmg);
    if (CurrentHP == 0)
      StartCoroutine(Die());

  }

  public static Vector2Int GenerateSquareRange(int factor) => new(factor * 2, factor);

  public IEnumerator ToIdleOrBlockState()
  {
    yield return MyStateManager.TransitionState(States[IsInBlockState() ? BlockState.Name : IdleState.Name]);
  }

  protected IEnumerator Die()
  {
    yield return MyStateManager.TransitionState(States[DeadState.Name]);
  }

  public IEnumerator DeathRitual()
  {
    if (MyStateManager.CurrentState.Hash == DeadState.Hash)
      yield return CurrentStateCoroutine();
  }

  public IEnumerator PerformAction(Skill skill)
  {
    yield return MyStateManager.TransitionState(skill.UnitState, skill.ActType == ActionType.NoClick);
  }

  public bool IsInBlockState() => MyStateManager.IsInBlockState();
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
