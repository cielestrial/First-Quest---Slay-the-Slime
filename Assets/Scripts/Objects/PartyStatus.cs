using System.Collections.Generic;
using UnityEngine;

public abstract class PartyStatus<T> : ScriptableObject
  where T : UnitController
{
  public bool isPlaying;
  [SerializeField] protected EventSystem MyEventSystem;

  public List<T> Units { get; private set; } = new(GridManager.MaxGridSize);

  public void AddUnit(T unit)
  {
    Units.Add(unit);
    if (isPlaying)
      MyEventSystem.MyUIEvent.AddUnit(unit);
  }

  public int GetPartyHp()
  {
    int partyHP = 0;
    foreach (T unit in Units)
      partyHP += unit.CurrentHP;
    return partyHP;
  }

  public Vector2Int GetStartLocation()
  {
    int startOffset = GridManager.DefaultRowSize * 2;
    int x = Mathf.FloorToInt(GridManager.MaxRowSize / 2);
    if (isPlaying) return new Vector2Int(x, startOffset);
    else return new Vector2Int(x, GridManager.MaxRowSize - 1 - startOffset);
  }

  public Quaternion GetStartQuaternion()
  {
    if (isPlaying) return Quaternion.identity;
    else return Quaternion.Euler(0, 180, 0);
  }
}