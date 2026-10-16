using System.Collections.Generic;
using UnityEngine;

namespace EventSys.Events
{

  public class BattleEvent : Event<PartyStatus<UnitController>>
  {
    public BattleEvent(int capacity) : base(capacity) { }

    public void DealDamage(UnitController attacker, List<UnitController> victims)
    {
      foreach (PartyStatus<UnitController> listener in Listeners)
      {
        // listener.ShowRanges();
      }
    }
  }

}