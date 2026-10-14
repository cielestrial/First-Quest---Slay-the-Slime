using System.Collections.Generic;
using UnityEngine;

public class AwakeningSkills : MonoBehaviour
{
  public const int SkillCount = 4;
  public Dictionary<string, int> SkillTree { get; protected set; } = new(SkillCount);
  public Dictionary<string, string> SkillDesc { get; protected set; } = new(SkillCount);
  public Dictionary<string, int> SkillTreeUnlock { get; protected set; } = new(SkillCount);
  public Dictionary<string, int> UpgradeTree { get; protected set; } = new(SkillCount);


}
