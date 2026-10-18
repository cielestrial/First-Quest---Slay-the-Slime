
// using Grid.Data;

// public class RogueController : HeroController
// {
//   public override void SetUp(
//     EventBus eventBus,
//     GridManager gridManager,
//     DetectionType detectionType,
//     TargetingSystem targetingSystem
//     )
//   {
//     base.SetUp(eventBus, gridManager, detectionType, targetingSystem);
//     UnitName = "Rogue";
//     // Level Requirement to learn
//     SkillTreeUnlock.Add("Move", 1);
//     SkillTreeUnlock.Add("Stab", 1);
//     SkillTreeUnlock.Add("Dagger Throw", 4);
//     SkillTreeUnlock.Add("Stealth", 10);
//     SkillTreeUnlock.Add("Wound", 16);
//     SkillTreeUnlock.Add("Knife Play", 24);
//     SkillTreeUnlock.Add("Awakening", 32);
//     // Skill Descriptions
//     SkillDesc.Add("Move", "Active: Move to a new location");
//     SkillDesc.Add("Stab", "Active: Good ol' reliable");
//     SkillDesc.Add("Dagger Throw", "Active: like playing darts");
//     SkillDesc.Add("Stealth", "Passive: Move passes through units");
//     SkillDesc.Add("Wound", "Passive: Stab lowers enemy defense");
//     SkillDesc.Add("Knife Play", "Passive: Stab will critical hit on an enemy marked by Dagger Throw");
//     SkillDesc.Add("Awakening", "Passive: Gain access to specialized skills");
//     // Skill Upgrade available every x levels from learn requirement level
//     UpgradeTree.Add("Stab", 5);
//     UpgradeTree.Add("Focus", 6);
//     UpgradeTree.Add("Magic Missile", 5);
//     UpgradeTree.Add("Wound", 6);
//     // Skill Tree, learnt skills and their level
//     SkillTree.Add("Move", 1);
//     SkillTree.Add("Stab", 1);
//   }

//   public void StabAction()
//   {

//   }

//   public void DaggerThrowAction()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Dagger Throw"])
//       return;


//   }

//   public void StealthAction()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Stealth"])
//       return;

//   }

//   public void WoundPassive()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Wound"])
//       return;

//   }

//   public void KnifePlayPassive()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Knife Play"])
//       return;

//   }

//   public void AwakeningPassive()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Awakening"])
//       return;

//   }
// }
