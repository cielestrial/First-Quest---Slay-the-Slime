using UnityEngine;

namespace EventSys.Events
{

  public class RangeEvent : Event<RangeManager>
  {
    public RangeEvent(int capacity) : base(capacity) { }

    public void ShowRange(Vector3 spawnLocation, Vector2Int factors)
    {
      foreach (RangeManager listener in Listeners)
        listener.ShowRanges(spawnLocation, factors);
    }
    public void HideRange()
    {
      foreach (RangeManager listener in Listeners)
        listener.HideRanges();
    }
  }

}