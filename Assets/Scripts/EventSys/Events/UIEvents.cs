namespace EventSys.Events
{

  public class UIEvent : Event<UIManager>
  {
    public UIEvent(int capacity) : base(capacity) { }

    public void AddUnit(UnitController unit)
    {
      foreach (UIManager listener in Listeners)
        listener.AddToUnitSelection(unit);
    }
    public void RemoveUnit(UnitController unit)
    {
      foreach (UIManager listener in Listeners)
        listener.RemoveFromUnitSelection(unit);
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
    public void HideUI()
    {
      foreach (UIManager listener in Listeners)
        listener.HideUI();
    }
    public void ShowUI()
    {
      foreach (UIManager listener in Listeners)
        listener.ShowUI();
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