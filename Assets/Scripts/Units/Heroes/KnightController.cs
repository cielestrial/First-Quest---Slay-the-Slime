using States.Knight;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class KnightController : HeroController
{
  void Awake()
  {
    UnitName = "Knight";
    MyAnimator = GetComponent<Animator>();

    // Level Requirement to learn
    SkillTreeUnlock.Add(MoveState.Name, 1);
    SkillTreeUnlock.Add("Slash", 1);
    SkillTreeUnlock.Add("Block", 4);
    SkillTreeUnlock.Add("Mega Slash", 8);
    SkillTreeUnlock.Add("Party Up", 10);
    SkillTreeUnlock.Add("Counter", 16);
    SkillTreeUnlock.Add("Awakening", 24);
    // Skill Descriptions
    SkillDesc.Add(MoveState.Name, "Active\nEmpty: Move to a new location");
    SkillDesc.Add("Slash", "Active\nEnemy: Deal single target damage");
    SkillDesc.Add("Block", "Active\nNone: Reduce incoming damage");
    SkillDesc.Add("Mega Slash", "Active\nEnemy: Deal AOE damage");
    SkillDesc.Add("Party Up", "Passive: Gain a new party member");
    SkillDesc.Add("Counter", "Passive: Upon succesful block retaliate with slash");
    SkillDesc.Add("Awakening", "Passive: Gain access to specialized skills");
    // Skill Type
    SkillActionType.Add(MoveState.Name, ActionType.EmptyClick);
    SkillActionType.Add("Slash", ActionType.EnemyClick);
    SkillActionType.Add("Block", ActionType.NoClick);
    SkillActionType.Add("Mega Slash", ActionType.EnemyClick);
    SkillActionType.Add("Party Up", ActionType.Idle);
    SkillActionType.Add("Counter", ActionType.Idle);
    SkillActionType.Add("Awakening", ActionType.Idle);
    // Skill Ranges 1 = 1, 2 = 3, 3 = 5, 4 = 7, 5 = 9...
    // Where (1, 1) is a cross and (2, 1) is a square
    SkillRange.Add(MoveState.Name, GenerateSquareRange(2));
    SkillRange.Add("Slash", new(1, 1));
    SkillRange.Add("Mega Slash", new(2, 1));
    // Skill cost
    SkillCost.Add(MoveState.Name, new(1));
    SkillCost.Add("Slash", new(1));
    SkillCost.Add("Block", new(1));
    SkillCost.Add("Mega Slash", new(2, 2));
    SkillCost.Add("Party Up", new(2));
    SkillCost.Add("Counter", new());
    SkillCost.Add("Awakening", new(2));
    // Skill Upgrade available every x levels from learn requirement level
    UpgradeTree.Add("Slash", 5);
    UpgradeTree.Add("Block", 5);
    UpgradeTree.Add("Mega Slash", 6);
    UpgradeTree.Add("Party Up", 10);
    UpgradeTree.Add("Counter", 5);
    // Skill Tree, learnt skills and their level
    SkillTree.Add(MoveState.Name, 1);
    SkillTree.Add("Slash", 1);
    // SkillTree.Add("Block", 1);
    SkillTree.Add("Mega Slash", 1);
    // SkillTree.Add("Party Up", 1);
    // SkillTree.Add("Counter", 1);
    // SkillTree.Add("Awakening", 1);

    // States
    States.Add(IdleState.Name, new IdleState(this));
    States.Add(MoveState.Name, new MoveState(this));
    States.Add(SlashState.Name, new SlashState(this));
    States.Add(BlockState.Name, new BlockState(this));
    States.Add(MegaSlashState.Name, new MegaSlashState(this));
    // States.Add("Party Up", 10);
    // States.Add("Counter", 16);
    // States.Add("Awakening", 24);

    MyStateManager = new KnightStateManager(States[IdleState.Name], this);
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
