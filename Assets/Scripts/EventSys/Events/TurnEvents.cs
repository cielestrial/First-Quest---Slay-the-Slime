namespace EventSys.Events
{

  public class TurnEvent : Event<TurnManager>
  {
    public TurnEvent(int capacity) : base(capacity) { }

    public void UnitSelected(UnitController unit)
    {
      foreach (TurnManager listener in Listeners)
        listener.RegisterUnit(unit);
    }
    public void ActionTaken()
    {
      foreach (TurnManager listener in Listeners)
        listener.AfterAction();
    }
    public void StopUnitCoroutine()
    {
      foreach (TurnManager listener in Listeners)
        listener.StopUnitCoroutine();
    }
  }

}