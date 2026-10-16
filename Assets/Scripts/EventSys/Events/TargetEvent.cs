

namespace EventSys.Events
{

  public class TargetEvent : Event<TargetingSystem>
  {
    public TargetEvent(int capacity) : base(capacity) { }

    public void FocusCameraOn(UnitController unit)
    {
      foreach (TargetingSystem listener in Listeners)
        listener.FocusCameraOnTarget(unit);
    }
  }

}