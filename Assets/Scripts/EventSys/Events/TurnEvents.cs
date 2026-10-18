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
        listener.TriggerActionPerformed();
    }
    public void AddGarbage(UnitController deceasedUnit)
    {
      foreach (TurnManager listener in Listeners)
        listener.AddGarbageForCleanup(deceasedUnit);
    }
    public void StopUnitCoroutine()
    {
      foreach (TurnManager listener in Listeners)
        listener.StopUnitCoroutine();
    }

    public void SetPlayerTurnEnder(UnitController unit)
    {
      foreach (TurnManager listener in Listeners)
        listener.SetPlayerTurnEnder(unit);
    }
    public void SetEnemyTurnEnder(UnitController unit)
    {
      foreach (TurnManager listener in Listeners)
        listener.SetEnemyTurnEnder(unit);
    }
  }
}