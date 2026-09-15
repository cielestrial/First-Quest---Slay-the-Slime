
public class KnightController : HeroController
{
  void Awake()
  {
    // Level Requirement to learn
    SkillTreeUnlock.Add("Move", 1);
    SkillTreeUnlock.Add("Slash", 1);
    SkillTreeUnlock.Add("Block", 4);
    SkillTreeUnlock.Add("Mega Slash", 8);
    SkillTreeUnlock.Add("Party Up", 10);
    SkillTreeUnlock.Add("Counter", 16);
    SkillTreeUnlock.Add("Awakening", 24);
    // Skill Descriptions
    SkillDesc.Add("Move", "Active\nEmpty: Move to a new location");
    SkillDesc.Add("Slash", "Active\nEnemy: Deal single target damage");
    SkillDesc.Add("Block", "Active\nNone: Reduce incoming damage");
    SkillDesc.Add("Mega Slash", "Active\nEnemy: Deal AOE damage");
    SkillDesc.Add("Party Up", "Passive: Gain a new party member");
    SkillDesc.Add("Counter", "Passive: Upon succesful block retaliate with slash");
    SkillDesc.Add("Awakening", "Passive: Gain access to specialized skills");
    // Skill Type
    SkillActionType.Add("Move", ActionType.EmptyClick);
    SkillActionType.Add("Slash", ActionType.EnemyClick);
    SkillActionType.Add("Block", ActionType.NoClick);
    SkillActionType.Add("Mega Slash", ActionType.EnemyClick);
    SkillActionType.Add("Party Up", ActionType.Idle);
    SkillActionType.Add("Counter", ActionType.Idle);
    SkillActionType.Add("Awakening", ActionType.Idle);
    // Skill Ranges 1 = 1, 2 = 3, 3 = 5, 4 = 7, 5 = 9...
    // Where (1, 1) is a cross and (2, 1) is a square
    SkillRange.Add("Move", GenerateSquareRange(2));
    SkillRange.Add("Slash", new(1, 1));
    SkillRange.Add("Mega Slash", new(2, 1));
    // Skill Upgrade available every x levels from learn requirement level
    UpgradeTree.Add("Slash", 5);
    UpgradeTree.Add("Block", 5);
    UpgradeTree.Add("Mega Slash", 6);
    UpgradeTree.Add("Party Up", 10);
    UpgradeTree.Add("Counter", 5);
    // Skill Tree, learnt skills and their level
    SkillTree.Add("Move", 1);
    SkillTree.Add("Slash", 1);
    // SkillTree.Add("Block", 1);
    SkillTree.Add("Mega Slash", 1);
    // SkillTree.Add("Party Up", 1);
    // SkillTree.Add("Counter", 1);
    // SkillTree.Add("Awakening", 1);
  }



  public override void PerformAction(string actionName)
  {
    CurrentAction = actionName;
    switch (CurrentAction)
    {
      case "Move":
        EmptyClick();
        break;

      case "Slash":
        EnemyClick();
        break;

      case "Mega Slash":
        if (CurrentLevel < SkillTreeUnlock["Mega Slash"])
          EnemyClick();
        break;

      case "Block":
        if (CurrentLevel < SkillTreeUnlock["Block"])
          NoClick();
        break;

      default:
        Idle();
        break;
    }
  }

  public void Passive(string _passive)
  {
    switch (_passive)
    {
      case "Party Up":
        if (CurrentLevel < SkillTreeUnlock["Party Up"])
          Idle();
        break;

      case "Counter":
        if (CurrentLevel < SkillTreeUnlock["Counter"])
          Idle();
        break;

      case "Awakening":
        if (CurrentLevel < SkillTreeUnlock["Awakening"])
          Idle();
        break;
    }

  }

}
