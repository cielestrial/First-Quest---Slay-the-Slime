
using Grid.Data;
using States.Slime;
using Unit.Data;

public class SlimeController : UnitController
{
  protected static new int CurrentAP = 2;
  protected static new int TotalAP = 2;

  public override int GetCurrentAP() => CurrentAP;
  public override int GetTotalAP() => TotalAP;

  public override void SetUp(
    EventBus eventBus,
    GridManager gridManager,
    DetectionType detectionType,
    TargetingSystem targetingSystem
    )
  {
    base.SetUp(eventBus, gridManager, detectionType, targetingSystem);
    UnitName = "Slime";

    // States
    States.Add(IdleState.Name, new IdleState(this));
    States.Add(DeadState.Name, new DeadState(this));
    States.Add(MoveState.Name, new MoveState(this));
    States.Add(BlockState.Name, new BlockState(this));

    // States.Add("Party Up", 10);
    // States.Add("Counter", 16);
    // States.Add("Awakening", 24);

    MyStateManager = new SlimeStateManager(States[IdleState.Name], this);

    // Level Requirement to learn
    SkillTree.Add(MoveState.Name, new Skill(
      MoveState.Name,
      "Active\nEmpty: Move to a new location",
      ActionType.EmptyClick,
      1,
      States[MoveState.Name],
      0f,
      GenerateSquareRange(2),
      new SkillResourceCost(1)
    ));

  }

  public override void ConsumeResources(SkillResourceCost cost)
  {
    CurrentAP = ConsumeResource(CurrentAP, cost.AP);
    CurrentMP = ConsumeResource(CurrentMP, cost.MP);
  }
  public override bool SufficientResources(SkillResourceCost cost)
    => SufficientResource(CurrentAP, cost.AP) && SufficientResource(CurrentMP, cost.MP);

  public override void ResetCurrentAP()
  {
    CurrentAP = TotalAP;
  }
  public override void IncreaseTotalAP(int byValue)
  {
    if (byValue > 0)
      TotalAP += byValue;
  }
}
