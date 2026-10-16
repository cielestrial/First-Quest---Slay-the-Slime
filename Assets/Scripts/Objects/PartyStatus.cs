using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PartyStatus<T> : ScriptableObject
  where T : UnitController
{
  public bool IsPlaying = false;
  protected EventBus eventBus;
  protected GridManager gridManager;
  protected TargetingSystem targetingSystem;

  public List<T> Units { get; private set; } = new(GridManager.MaxGridSize);

  public void AddDependencies(
    EventBus eventBus,
    GridManager gridManager,
    TargetingSystem targetingSystem
    )
  {
    this.eventBus = eventBus;
    this.gridManager = gridManager;
    this.targetingSystem = targetingSystem;
  }

  public void ClearDependencies()
  {
    targetingSystem = null;
    gridManager = null;
    eventBus = null;
  }

  public void AddUnit(T unit)
  {
    Units.Add(unit);
    if (IsPlaying)
      eventBus.MyUIEvent.AddUnit(unit);
  }

  public int GetPartyHp()
  {
    int partyHP = 0;
    foreach (T unit in Units)
      partyHP += unit.CurrentHP;
    return partyHP;
  }

  public abstract void SpawnInitialUnit(Scene scene);

  public Vector2Int GetStartLocation()
  {
    int startOffset = GridManager.DefaultRowSize * 2;
    int x = Mathf.FloorToInt(GridManager.MaxRowSize / 2);
    if (IsPlaying) return new Vector2Int(x, startOffset);
    else return new Vector2Int(x, GridManager.MaxRowSize - 1 - startOffset);
  }

  public Quaternion GetStartQuaternion()
  {
    if (IsPlaying) return Quaternion.identity;
    else return Quaternion.Euler(0, 180, 0);
  }
}