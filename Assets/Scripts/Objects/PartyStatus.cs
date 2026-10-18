using System;
using System.Collections.Generic;
using Grid.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PartyStatus<T> : ScriptableObject
  where T : UnitController
{
  [NonSerialized] public bool IsPlaying;
  protected EventBus eventBus;
  protected GridManager gridManager;
  protected TargetingSystem targetingSystem;

  protected List<T> Units;
  const int ReviveLimit = 4;
  protected List<T> DeadUnits;

  public void SetUp(
    EventBus eventBus,
    GridManager gridManager,
    TargetingSystem targetingSystem
    )
  {
    this.eventBus = eventBus;
    this.gridManager = gridManager;
    this.targetingSystem = targetingSystem;
    IsPlaying = false;
    Units = new(GridManager.MaxGridSize);
    DeadUnits = new(ReviveLimit);
  }

  public void TearDown()
  {
    DeadUnits?.Clear();
    Units?.Clear();
    IsPlaying = false;
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

  public void StoreDeadUnit(T unit)
  {
    Units.Remove(unit);
    unit.gameObject.SetActive(false);

    if (DeadUnits.Count < ReviveLimit)
      DeadUnits.Add(unit);

    else if (DeadUnits.Count == ReviveLimit)
    {
      int weakest = 0;
      for (int i = 1; i < ReviveLimit; ++i)
        if (DeadUnits[i].CurrentLevel < DeadUnits[weakest].CurrentLevel)
          weakest = i;

      if (unit.CurrentLevel < DeadUnits[weakest].CurrentLevel)
        Destroy(unit.gameObject);
      else
      {
        T garbage = DeadUnits[weakest];
        DeadUnits.RemoveAt(weakest);
        DeadUnits.Add(unit);
        Destroy(garbage.gameObject);
      }
    }
  }

  public int GetPartyHp()
  {
    int partyHP = 0;
    foreach (T unit in Units)
      partyHP += unit.CurrentHP;
    return partyHP;
  }

  public abstract void SpawnInitialUnit(Scene scene);

  protected Vector2Int GetStartLocation()
  {
    int startOffset = GridManager.DefaultRowSize * 2;
    int x = Mathf.FloorToInt(GridManager.MaxRowSize / 2);
    if (IsPlaying) return new Vector2Int(x, startOffset);
    else return new Vector2Int(x, GridManager.MaxRowSize - 1 - startOffset);
  }

  protected Quaternion GetStartQuaternion()
  {
    if (IsPlaying) return Quaternion.identity;
    else return Quaternion.Euler(0, 180, 0);
  }

  protected DetectionType GetDetectionType()
  => IsPlaying ? DetectionType.Ally : DetectionType.Enemy;

  protected void SetInitialTurnEnder(UnitController unit)
  {
    if (IsPlaying)
      eventBus.MyTurnEvent.SetPlayerTurnEnder(unit);
    else
      eventBus.MyTurnEvent.SetEnemyTurnEnder(unit);
  }
}