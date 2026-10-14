using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States.Knight
{
  public class MegaSlashState : SlashState
  {
    public new const string Name = "Mega Slash";
    public static new readonly int Hash = Animator.StringToHash(Name);

    public MegaSlashState(UnitController unit) : base(unit)
    {
      SetName(Name);
      SetHash(Hash);
    }
  }

}