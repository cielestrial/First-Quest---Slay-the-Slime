using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
  private EventBus eventBus;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  private HeroPartyStatus heroParty;
  private SlimePartyStatus slimeParty;
  public UnitController SelectedUnit { get; private set; } = null; // all units in turn order, player + AI mixed
  public bool PlayerTurn { get; private set; }
  public int PlayerTurnCount { get; private set; } = 0;
  public const float GameSpeed = 1.3f;
  private Coroutine mainCoroutine;
  private Coroutine unitCoroutine;
  private static readonly WaitForSeconds waitForSeconds = new(1f);

  public void AddDependencies(
    EventBus eventBus,
    SlimePartyStatus slimeParty,
    HeroPartyStatus heroParty
    )
  {
    this.eventBus = eventBus;
    this.slimeParty = slimeParty;
    this.heroParty = heroParty;
  }
  public void ClearDependencies()
  {
    heroParty = null;
    slimeParty = null;
    eventBus = null;
  }
  public void AddListener()
  {
    eventBus.MyTurnEvent?.AddListener(this);
  }
  public void RemoveListener()
  {
    eventBus.MyTurnEvent?.RemoveListener(this);
  }

  public void StartBattle()
  {
    // Slimes get first move
    PlayerTurn = slimeParty.IsPlaying;
    mainCoroutine = StartCoroutine(RunTurns());
  }
  public void StopBattle()
  {
    StopCoroutine(mainCoroutine);
    StopAllCoroutines();
    PlayerTurn = false;
  }

  private bool IsGameOver()
  => heroParty.IsPlaying && heroParty.GetPartyHp() <= 0 ||
  slimeParty.IsPlaying && slimeParty.GetPartyHp() <= 0;


  IEnumerator RunTurns()
  {
    while (!IsGameOver())
    {
      if (PlayerTurn)
      {
        // Select Unit 
        yield return StartCoroutine(WaitForUnitSelection());
        // Select Action
        if (SelectedUnit != null)
          yield return StartCoroutine(WaitForUnitAction());
      }
      else
      {
        yield return StartCoroutine(WaitForEnemy());
        PlayerTurn = true;
      }
      yield return null;
    }

    IEnumerator WaitForUnitSelection()
    {
      while (SelectedUnit == null)
        yield return null;
    }

    IEnumerator WaitForUnitAction()
    {
      while (SelectedUnit.IsInIdleOrBlockState())
        yield return null;

      eventBus.Snapshot();
      unitCoroutine = StartCoroutine(SelectedUnit.CurrentStateCoroutine());
      yield return unitCoroutine;
    }

    IEnumerator WaitForEnemy()
    {
      yield return waitForSeconds;

      // Vector2Int targetCell = unit.DecideAIMove();
      // List<Vector2Int> path = Pathfinder.FindPath(unit.GridPos, targetCell);

      // yield return StartCoroutine(unit.MoveAlongPath(path));
    }

  }

  public void RegisterUnit(UnitController unit) { SelectedUnit = unit; }
  public void UnregisterUnit() { SelectedUnit = null; }

  public void StopUnitCoroutine() { StopCoroutine(unitCoroutine); }

  public void AfterAction()
  {
    StartCoroutine(SelectedUnit.PerformAction(IdleState.Name));
    eventBus.MyUIEvent.UpdateUnitInfo(SelectedUnit);
    if (SelectedUnit.GetCurrentAP() == 0)
      EndPlayerTurn();
  }

  public void EndPlayerTurn()
  {
    PlayerTurnCount++;
    PlayerTurn = false;
    SelectedUnit.ResetCurrentAP();
    eventBus.MyUIEvent.ClearUnitInfoPanel();
    eventBus.MyUIEvent.ClearUnitActionsPanel();
    UnregisterUnit();
  }
}
