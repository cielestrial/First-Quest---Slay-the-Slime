using UnityEngine;

namespace Unit.Data
{
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

  // public Dictionary<string, int> UpgradeTree { get; protected set; } = new(15);
  // Level system for skills?
  public struct Skill
  {
    public readonly string Name;
    public readonly string Description;
    public readonly ActionType ActType;
    public readonly int LevelReq;
    public readonly int Level;
    public readonly State UnitState;
    // Skill Ranges 1 = 1, 2 = 3, 3 = 5, 4 = 7, 5 = 9...
    // Where (1, 1) is a cross and (2, 1) is a square
    public readonly float AtkMod;
    public Vector2Int Range;
    public SkillResourceCost Cost;

    public Skill(
      string name,
      string description,
      ActionType actType,
      int levelReq,
      State unitState,
      float atkMod,
      Vector2Int range,
      SkillResourceCost cost
      )
    {
      Name = name;
      Description = description;
      ActType = actType;
      LevelReq = levelReq;
      Level = 1;
      UnitState = unitState;
      AtkMod = atkMod;
      Range = range;
      Cost = cost;
    }
  }

}