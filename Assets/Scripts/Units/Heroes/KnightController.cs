using Grid.Data;
using States.Knight;
using Unit.Data;
using UnityEngine;

public class KnightController : HeroController
{
  public override void SetUp(
    EventBus eventBus,
    GridManager gridManager,
    DetectionType detectionType,
    TargetingSystem targetingSystem
    )
  {
    base.SetUp(eventBus, gridManager, detectionType, targetingSystem);
    UnitName = "Knight";

    // States
    States.Add(IdleState.Name, new IdleState(this));
    States.Add(DeadState.Name, new DeadState(this));
    States.Add(MoveState.Name, new MoveState(this));
    States.Add(SlashState.Name, new SlashState(this));
    States.Add(BlockState.Name, new BlockState(this));
    States.Add(MegaSlashState.Name, new MegaSlashState(this));
    // States.Add("Party Up", 10);
    // States.Add("Counter", 16);
    // States.Add("Awakening", 24);

    MyStateManager = new KnightStateManager(States[IdleState.Name], this);

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

    SkillTree.Add(SlashState.Name, new Skill(
      SlashState.Name,
      "Active\nEnemy: Deal single target damage",
      ActionType.EnemyClick,
      1,
      States[SlashState.Name],
      1f,
      new Vector2Int(1, 1),
      new SkillResourceCost(1)
    ));

    SkillTree.Add(BlockState.Name, new Skill(
      BlockState.Name,
      "Active\nNone: Reduce incoming damage",
      ActionType.NoClick,
      4,
      States[BlockState.Name],
      0f,
      Vector2Int.zero,
      new SkillResourceCost(1)
    ));

    SkillTree.Add(MegaSlashState.Name, new Skill(
      MegaSlashState.Name,
      "Active\nEnemy: Deal AOE damage",
      ActionType.EnemyClick,
      8,
      States[MegaSlashState.Name],
      2f,
      new Vector2Int(2, 1),
      new SkillResourceCost(2, 2)
    ));

    SkillTree.Add("Party Up", new Skill(
      "Party Up",
      "Passive: Gain a new party member",
      ActionType.Idle,
      10,
      States[IdleState.Name],
      0f,
      Vector2Int.zero,
      new SkillResourceCost(2)
    ));

    SkillTree.Add("Counter", new Skill(
      "Counter",
      "Passive: Upon succesful block retaliate with slash",
      ActionType.Idle,
      16,
      States[IdleState.Name],
      0f,
      Vector2Int.zero,
      new SkillResourceCost(0)
    ));

    SkillTree.Add("Awakening", new Skill(
      "Awakening",
      "Passive: Gain access to specialized skills",
      ActionType.Idle,
      24,
      States[IdleState.Name],
      0f,
      Vector2Int.zero,
      new SkillResourceCost(2)
    ));

    // Skill Upgrade available every x levels from learn requirement level
    // UpgradeTree.Add("Slash", 5);
    // UpgradeTree.Add("Block", 5);
    // UpgradeTree.Add("Mega Slash", 6);
    // UpgradeTree.Add("Party Up", 10);
    // UpgradeTree.Add("Counter", 5);

    // Skill Tree, learnt skills and their level
    Actives.Add(MoveState.Name, SkillTree[MoveState.Name]);
    Actives.Add(SlashState.Name, SkillTree[SlashState.Name]);
    Actives.Add(BlockState.Name, SkillTree[BlockState.Name]);
    Actives.Add(MegaSlashState.Name, SkillTree[MegaSlashState.Name]);
    // Passives.Add("Party Up", 1);
    // Passives.Add("Counter", 1);
    // Passives.Add("Awakening", 1);
  }



  // public override void PerformAction(string actionName)
  // {
  //   CurrentAction = actionName;
  //   switch (CurrentAction)
  //   {
  //     case "Move":
  //       EmptyClick();
  //       break;

  //     case "Slash":
  //       EnemyClick();
  //       break;

  //     case "Mega Slash":
  //       if (CurrentLevel < SkillTreeUnlock["Mega Slash"])
  //         EnemyClick();
  //       break;

  //     case "Block":
  //       if (CurrentLevel < SkillTreeUnlock["Block"])
  //         NoClick();
  //       break;

  //     default:
  //       Idle();
  //       break;
  //   }
  // }

  // public void Passive(string _passive)
  // {
  //   switch (_passive)
  //   {
  //     case "Party Up":
  //       if (CurrentLevel < SkillTreeUnlock["Party Up"])
  //         Idle();
  //       break;

  //     case "Counter":
  //       if (CurrentLevel < SkillTreeUnlock["Counter"])
  //         Idle();
  //       break;

  //     case "Awakening":
  //       if (CurrentLevel < SkillTreeUnlock["Awakening"])
  //         Idle();
  //       break;
  //   }

  // }

}
