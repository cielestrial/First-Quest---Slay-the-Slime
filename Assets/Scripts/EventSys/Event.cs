using System.Collections.Generic;

namespace EventSys.Events
{

  public abstract class Event<Manager>
  {
    public readonly HashSet<Manager> Listeners;

    protected Event(int capacity)
    {
      Listeners = new(capacity);
    }
    public void AddListener(Manager listener)
    {
      Listeners.Add(listener);
    }
    public void RemoveListener(Manager listener)
    {
      Listeners.Remove(listener);
    }
    public void ClearListeners()
    {
      Listeners.Clear();
    }
  }

}