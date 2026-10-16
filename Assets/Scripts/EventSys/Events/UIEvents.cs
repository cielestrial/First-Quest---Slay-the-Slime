namespace EventSys.Events
{

  public class UIEvent : Event<UIManager>
  {
    public UIEvent(int capacity) : base(capacity) { }

    public void AddUnit(UnitController unit)
    {
      foreach (UIManager listener in Listeners)
        listener.AddUnitSelectionButton(unit);
    }
    public void UpdateUnitInfo(UnitController unit)
    {
      foreach (UIManager listener in Listeners)
        listener.UpdateUnitInfoPanel(unit);
    }
    public void ShowActions(UnitController unit)
    {
      foreach (UIManager listener in Listeners)
        listener.ShowActions(unit);
    }

    public void ClearUnitInfoPanel()
    {
      foreach (UIManager listener in Listeners)
        listener.ClearUnitInfoPanel();
    }
    public void ClearUnitActionsPanel()
    {
      foreach (UIManager listener in Listeners)
        listener.ClearUnitActionsPanel();
    }
    public void ClearActionDescription()
    {
      foreach (UIManager listener in Listeners)
        listener.ClearActionDescription();
    }
  }

}