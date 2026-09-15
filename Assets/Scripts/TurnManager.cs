using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnManager : MonoBehaviour
{
  public static TurnManager Instance { get; private set; }

  [SerializeField] private GridManager MyGridManager;
  [SerializeField] private TargetingSystem MyTargetingSystem;
  [SerializeField] private RangeManager MyRangeManager;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  [SerializeField] private HeroPartyStatus MyHeroParty;
  [SerializeField] private SlimePartyStatus MySlimeParty;
  public UnitController SelectedUnit = null; // all units in turn order, player + AI mixed
  public bool PlayerTurn { get; private set; }
  private string prevAction;
  private bool actionTaken = false;

  void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }
    Instance = this;
  }

  void Start()
  {
    PlayerTurn = MySlimeParty.isPlaying;
    StartCoroutine(RunTurns());
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
        {
          yield return StartCoroutine(WaitForUnitAction());
          if (actionTaken)
            AfterAction();
          if (SelectedUnit.GetCurrentAP() == 0)
            EndPlayerTurn();
        }
      }
      else
      {
        yield return StartCoroutine(WaitForEnemy());
        PlayerTurn = true;
      }
      yield return null;
    }

    bool IsGameOver()
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

    IEnumerator WaitForUnitSelection()
    {
      while (SelectedUnit == null)
        yield return null;
    }

    IEnumerator WaitForUnitAction()
    {
      while (SelectedUnit.IsIdle())
        yield return null;
      /* 
      Action selected:
      * NoClick = 1,
      * FreeClick = 2,
      * EmptyClick = 3,
      * EnemyClick = 4
      */
      switch (SelectedUnit.CurrentActionType)
      {
        case UnitController.ActionType.NoClick:
          SelectedUnit.NoClick();
          break;

        case UnitController.ActionType.FreeClick:
          yield return StartCoroutine(WaitForGridSelection(false));
          break;

        case UnitController.ActionType.EmptyClick:
          yield return StartCoroutine(WaitForGridSelection(true));
          break;

        case UnitController.ActionType.EnemyClick:
          yield return StartCoroutine(WaitForGridSelection(true));
          break;

        default:
          break;
      }
    }

    void AfterAction()
    {
      actionTaken = false;
      SelectedUnit.Idle();
      SelectedUnit.SetCurrentAP(SelectedUnit.GetCurrentAP() - 1);
      UIManager.Instance.UpdateUnitInfoPanel(SelectedUnit);
    }
    void EndPlayerTurn()
    {
      PlayerTurn = false;
      SelectedUnit.ResetCurrentAP();
      UIManager.Instance.ClearUnitInfoPanel();
      UIManager.Instance.ClearUnitActionsPanel();
      SelectedUnit = null;
    }

    /**
      Only works for move atm
     */
    IEnumerator WaitForGridSelection(bool withDetection)
    {
      MyRangeManager.ShowRanges(MyGridManager, SelectedUnit.transform.position, SelectedUnit.SkillRange[SelectedUnit.CurrentAction]);

      bool locationChosen = false;
      Vector2Int start, end;
      List<Vector2Int> path = new(GridManager.MaxGridSize);
      prevAction = SelectedUnit.CurrentAction;
      while (!locationChosen && SelectedUnit.CurrentAction == prevAction)
      {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
          Vector3? targetLocation = MyTargetingSystem.GetTargetLocation();
          if (targetLocation != null)
          {
            start = GridManager.ConvertWorldPosToGrid(SelectedUnit.transform.position);
            end = GridManager.ConvertWorldPosToGrid((Vector3)targetLocation);
            path = MyGridManager.FindMovePath(start, end, withDetection);
            if (path.Count > 0)
              locationChosen = true;
          }
        }
        yield return null;
      }

      MyRangeManager.HideRanges();
      if (locationChosen)
      {
        yield return StartCoroutine(SelectedUnit.Move(path));
        // Move chosen
        actionTaken = true;
      }
    }

    IEnumerator WaitForEnemy()
    {
      yield return new WaitForSeconds(1f);

      // Vector2Int targetCell = unit.DecideAIMove();
      // List<Vector2Int> path = Pathfinder.FindPath(unit.GridPos, targetCell);

      // yield return StartCoroutine(unit.MoveAlongPath(path));
    }

  }
}
