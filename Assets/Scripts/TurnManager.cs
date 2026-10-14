using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
  [SerializeField] private EventSystem MyEventSystem;
  [SerializeField] private RangeManager MyRangeManager;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  [SerializeField] private HeroPartyStatus MyHeroParty;
  [SerializeField] private SlimePartyStatus MySlimeParty;
  public UnitController SelectedUnit { get; private set; } = null; // all units in turn order, player + AI mixed
  public bool PlayerTurn { get; private set; }
  public int PlayerTurnCount { get; private set; } = 0;
  public const float GameSpeed = 1.3f;

  private static readonly WaitForSeconds waitForSeconds = new(1f);

  void Start()
  {
    MyEventSystem.MyTurnEvent.AddListener(this);
    PlayerTurn = MySlimeParty.isPlaying;
    StartCoroutine(RunTurns());
  }

  void OnEnable()
  {
    MyEventSystem.MyTurnEvent.AddListener(this);
  }

  void OnDisable()
  {
    StopAllCoroutines();
    MyEventSystem.MyTurnEvent?.RemoveListener(this);
    PlayerTurn = false;
  }

  private bool IsGameOver()
  {
    if (MyHeroParty.isPlaying)
    {
      return MyHeroParty.GetPartyHp() <= 0;
    }
    else if (MySlimeParty.isPlaying)
    {
      return MySlimeParty.GetPartyHp() <= 0;
    }
    else
      return false;
  }

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

      MyEventSystem.Snapshot();
      yield return StartCoroutine(SelectedUnit.CurrentStateCoroutine());
      /* 
      Action selected:
      * NoClick = 1,
      * FreeClick = 2,
      * EmptyClick = 3,
      * EnemyClick = 4
      */
      // switch (SelectedUnit.CurrentActionType)
      // {
      //   case UnitController.ActionType.NoClick:
      //     SelectedUnit.NoClick();
      //     break;

      //   case UnitController.ActionType.FreeClick:
      //     yield return StartCoroutine(WaitForGridSelection(false));
      //     break;

      //   case UnitController.ActionType.EmptyClick:
      //     yield return StartCoroutine(WaitForGridSelection(true));
      //     break;

      //   case UnitController.ActionType.EnemyClick:
      //     yield return StartCoroutine(WaitForGridSelection(true));
      //     break;

      //   default:
      //     break;
      // }
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

  public void AfterAction()
  {
    StartCoroutine(SelectedUnit.PerformAction(IdleState.Name));
    MyEventSystem.MyUIEvent.UpdateUnitInfo(SelectedUnit);
    if (SelectedUnit.GetCurrentAP() == 0)
      EndPlayerTurn();
  }

  public void EndPlayerTurn()
  {
    PlayerTurnCount++;
    PlayerTurn = false;
    SelectedUnit.ResetCurrentAP();
    MyEventSystem.MyUIEvent.ClearUnitInfoPanel();
    MyEventSystem.MyUIEvent.ClearUnitActionsPanel();
    UnregisterUnit();
  }
}
