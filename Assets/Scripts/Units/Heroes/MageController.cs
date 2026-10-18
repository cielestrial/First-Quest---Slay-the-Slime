
// using Grid.Data;

// public class MageController : HeroController
// {
//   public override void SetUp(
//     EventBus eventBus,
//     GridManager gridManager,
//     DetectionType detectionType,
//     TargetingSystem targetingSystem
//     )
//   {
//     base.SetUp(eventBus, gridManager, detectionType, targetingSystem);
//     UnitName = "Mage";
//     // Level Requirement to learn
//     SkillTreeUnlock.Add("Move", 1);
//     SkillTreeUnlock.Add("Whack", 1);
//     SkillTreeUnlock.Add("First Aid", 4);
//     SkillTreeUnlock.Add("Focus", 8);
//     SkillTreeUnlock.Add("Magic Missile", 10);
//     SkillTreeUnlock.Add("Gift", 16);
//     SkillTreeUnlock.Add("Awakening", 30);
//     // Skill Descriptions
//     SkillDesc.Add("Move", "Active: Move to a new location");
//     SkillDesc.Add("Whack", "Active: Throw hands");
//     SkillDesc.Add("First Aid", "Active: A band-aid solution\n(Ally)");
//     SkillDesc.Add("Focus", "Active: Recover MP\n(Self)");
//     SkillDesc.Add("Magic Missile", "Active: Ranged single target damage");
//     SkillDesc.Add("Gift", "Passive: Focus can now be used on allies\n(Ally)");
//     SkillDesc.Add("Awakening", "Passive: Gain access to specialized skills");
//     // Skill Upgrade available every x levels from learn requirement level
//     UpgradeTree.Add("Heal", 5);
//     UpgradeTree.Add("Focus", 6);
//     UpgradeTree.Add("Magic Missile", 5);
//     UpgradeTree.Add("Gift", 6); // applies to all members in range at max
//     // Skill Tree, learnt skills and their level
//     SkillTree.Add("Move", 1);
//     SkillTree.Add("Whack", 1);
//   }


//   public void WhackAction()
//   {

//   }

//   public void FirstAidAction()
//   {
//     if (CurrentLevel < SkillTreeUnlock["First Aid"])
//       return;


//   }

//   public void FocusAction()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Focus"])
//       return;

//   }

//   public void MagicMissileAction()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Magic Missile"])
//       return;

//   }

//   public void GiftPassive()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Gift"])
//       return;

//   }

//   public void AwakeningPassive()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Awakening"])
//       return;

//   }
// }
