using System.Collections.Generic;
using UnityEngine;

namespace EventSys.Events
{

  public class BattleEvent : Event<BattleSystem>
  {
    public BattleEvent(int capacity) : base(capacity) { }

    public void DamageDealt(UnitController by, float with, List<Vector2Int> to)
    {
      foreach (BattleSystem listener in Listeners)
        listener.DamageDealt(by, with, to);
    }

    public void UnitDeath(UnitController deceased)
    {
      foreach (BattleSystem listener in Listeners)
        listener.UnitDeath(deceased);
    }
  }

}