using System.Collections.Generic;
using Unit.Data;
using UnityEngine;

public class AwakeningSkills : MonoBehaviour
{
  public const int SkillCount = 4;
  public Dictionary<string, State> States { get; protected set; } = new(15);
  public Dictionary<string, Skill> SkillTree { get; protected set; } = new(15);
  public Dictionary<string, Skill> Actives { get; protected set; } = new(15);
  public Dictionary<string, Skill> Passives { get; protected set; } = new(15);


}
