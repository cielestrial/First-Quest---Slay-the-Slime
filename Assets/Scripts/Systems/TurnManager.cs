using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
  private EventBus eventBus;
  private HeroPartyStatus heroParty;
  private SlimePartyStatus slimeParty;
  // all units in turn order, player + AI mixed
  public UnitController SelectedUnit { get; private set; } = null;
  public bool PlayerTurn { get; private set; }
  public int PlayerTurnCount { get; private set; }
  public const float GameSpeed = 1.3f;
  private Coroutine mainCoroutine = null;
  private Coroutine unitCoroutine = null;
  private static readonly WaitForSeconds waitForSeconds = new(1f);
  private bool actionPerformed;
  private Stack<UnitController> garbage;
  private UnitController playerTurnEnder = null;
  private UnitController enemyTurnEnder = null;

  public void SetUp(
    EventBus eventBus,
    SlimePartyStatus slimeParty,
    HeroPartyStatus heroParty
    )
  {
    this.eventBus = eventBus;
    this.slimeParty = slimeParty;
    this.heroParty = heroParty;
    AddListener();
    garbage = new(GridManager.MaxGridSize);
  }
  public void TearDown()
  {
    garbage?.Clear();
    RemoveListener();
    heroParty = null;
    slimeParty = null;
    eventBus = null;
  }
  void AddListener()
  {
    eventBus.MyTurnEvent?.AddListener(this);
  }
  void RemoveListener()
  {
    if (eventBus != null)
      eventBus.MyTurnEvent?.RemoveListener(this);
  }

  public void StartBattle()
  {
    // Slimes get first move
    PlayerTurnCount = 0;
    PlayerTurn = slimeParty.IsPlaying;
    mainCoroutine = StartCoroutine(RunTurns());
  }
  public void StopBattle()
  {
    if (mainCoroutine != null)
      StopCoroutine(mainCoroutine);
    StopAllCoroutines();
    PlayerTurn = false;
  }

  private bool IsGameOver()
    => heroParty.GetPartyHp() <= 0 || slimeParty.GetPartyHp() <= 0;


  IEnumerator RunTurns()
  {
    while (!IsGameOver())
    {
      if (PlayerTurn)
      {
        // Show UI
        eventBus.MyTargetEvent.FocusCameraOn(playerTurnEnder);
        eventBus.MyUIEvent.ShowUI();
        // Select Unit 
        yield return StartCoroutine(WaitForUnitSelection());
        // Select Action
        if (SelectedUnit != null)
          yield return StartCoroutine(WaitForUnitAction());
        if (actionPerformed)
          yield return AfterAction();
      }
      else
      {
        eventBus.MyTargetEvent.FocusCameraOn(enemyTurnEnder);
        yield return StartCoroutine(WaitForEnemy());
        PlayerTurn = true;
      }
      yield return null;
    }
  }


  public void RegisterUnit(UnitController unit) { SelectedUnit = unit; }
  public void UnregisterUnit() { SelectedUnit = null; }

  public void StopUnitCoroutine()
  {
    if (unitCoroutine != null)
      StopCoroutine(unitCoroutine);
  }

  IEnumerator WaitForUnitSelection()
  {
    while (SelectedUnit == null)
      yield return null;
  }
  IEnumerator WaitForUnitAction()
  {
    actionPerformed = false;
    while (SelectedUnit.IsInIdleOrBlockState())
      yield return null;

    unitCoroutine = StartCoroutine(SelectedUnit.CurrentStateCoroutine());
    yield return unitCoroutine;
  }
  IEnumerator WaitForEnemy()
  {
    yield return null;
    slimeParty.AIUnitSelection();
    yield return null;
    eventBus.MyTargetEvent.FocusCameraOn(enemyTurnEnder);
    yield return waitForSeconds;
    enemyTurnEnder = SelectedUnit;
  }
  IEnumerator AfterAction()
  {
    Coroutine aftermath = StartCoroutine(Aftermath());
    Coroutine idle = StartCoroutine(SelectedUnit.ToIdleOrBlockState());
    eventBus.MyUIEvent.UpdateUnitInfo(SelectedUnit);

    yield return aftermath;
    yield return idle;

    if (SelectedUnit.GetCurrentAP() == 0)
      EndPlayerTurn();
  }
  IEnumerator Aftermath()
  {
    int remaining = garbage.Count;
    while (garbage.Count > 0)
      StartCoroutine(GarbageCleanup(garbage.Pop(), () => remaining--));
    yield return new WaitUntil(() => remaining == 0);
  }

  IEnumerator GarbageCleanup(UnitController deceasedUnit, Action OnDone)
  {
    try { yield return StartCoroutine(deceasedUnit.DeathRitual()); }
    finally { OnDone?.Invoke(); }
  }

  public void TriggerActionPerformed()
    => actionPerformed = true;

  public void AddGarbageForCleanup(UnitController deceasedUnit)
    => garbage.Push(deceasedUnit);

  public void EndPlayerTurn()
  {
    PlayerTurnCount++;
    PlayerTurn = false;
    playerTurnEnder = SelectedUnit;
    SelectedUnit.ResetCurrentAP();
    eventBus.MyUIEvent.ClearUnitInfoPanel();
    eventBus.MyUIEvent.ClearUnitActionsPanel();
    UnregisterUnit();
  }

  public void SetPlayerTurnEnder(UnitController unit)
    => playerTurnEnder = unit;

  public void SetEnemyTurnEnder(UnitController unit)
    => enemyTurnEnder = unit;

}
