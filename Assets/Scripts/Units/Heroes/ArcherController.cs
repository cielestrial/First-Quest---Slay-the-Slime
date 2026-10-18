
// using Grid.Data;

// public class ArcherController : HeroController
// {
//   public override void SetUp(
//     EventBus eventBus,
//     GridManager gridManager,
//     DetectionType detectionType,
//     TargetingSystem targetingSystem
//     )
//   {
//     base.SetUp(eventBus, gridManager, detectionType, targetingSystem);
//     UnitName = "Archer";
//     // Level Requirement to learn
//     SkillTreeUnlock.Add("Move", 1);
//     SkillTreeUnlock.Add("True Aim; Aim True", 1);
//     SkillTreeUnlock.Add("Volley", 4);
//     SkillTreeUnlock.Add("Focus", 8);
//     SkillTreeUnlock.Add("Birds & Stone", 10);
//     SkillTreeUnlock.Add("Flow State", 16);
//     SkillTreeUnlock.Add("Awakening", 30);
//     // Skill Descriptions
//     SkillDesc.Add("Move", "Active: Move to a new location");
//     SkillDesc.Add("True Aim; Aim True", "Active: Shoot an arrow that prioritizes knees");
//     SkillDesc.Add("Volley", "Active: Shoot an arrow right into the enemy back line");
//     SkillDesc.Add("Focus", "Active: Recover MP (Self)");
//     SkillDesc.Add("Birds & Stone", "Active: Shoot an arrow that pierces enemies");
//     SkillDesc.Add("Flow State", "Passive: Attacking after focus will critically hit (Self)");
//     SkillDesc.Add("Awakening", "Passive: Gain access to specialized skills");
//     // Skill Upgrade available every x levels from learn requirement level
//     UpgradeTree.Add("True Aim", 5);
//     UpgradeTree.Add("Volley", 5);
//     UpgradeTree.Add("Focus", 6);
//     UpgradeTree.Add("Birds & Stone", 5);
//     UpgradeTree.Add("Flow State", 6);
//     // Skill Tree, learnt skills and their level
//     SkillTree.Add("Move", 1);
//     SkillTree.Add("True Aim", 1);
//   }


//   public void TrueAimAimTrueAction()
//   {

//   }

//   public void VolleyAction()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Volley"])
//       return;


//   }

//   public void FocusAction()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Focus"])
//       return;

//   }

//   public void BirdsAndStoneAction()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Birds & Stone"])
//       return;

//   }

//   public void FlowStatePassive()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Flow State"])
//       return;

//   }

//   public void AwakeningPassive()
//   {
//     if (CurrentLevel < SkillTreeUnlock["Awakening"])
//       return;

//   }
// }
