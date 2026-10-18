using System.Collections.Generic;
using TMPro;
using Unit.Data;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class UIManager : MonoBehaviour
{
  private Canvas canvas;
  private EventBus eventBus;
  // [SerializeField] private TurnManager MyTurnManager;
  public Button UnitButton;
  public Button ActionButton;
  public Transform UnitSelectParent;
  public Transform UnitActionsParent;
  private readonly List<UnitSelect> unitSelection = new(GridManager.MaxGridSize);
  private readonly HashSet<string> unitSelectionKeys = new(GridManager.MaxGridSize);
  private readonly List<ActionSelect> actionSelection = new(15);
  private readonly HashSet<string> actionSelectionKeys = new(15);

  public TMP_Text UnitNameDisplay;
  public UnitStatsDisplay HPDisplay, MPDisplay, APDisplay;
  public TMP_Text ActionInfoDisplay;
  public TMP_Text ActionDescriptionDisplay;
  private UnitController prevUnit = null;

  public void SetUp(EventBus eventBus)
  {
    this.eventBus = eventBus;
    AddListener();

    canvas = GetComponent<Canvas>();
    HideUI();
  }
  public void TearDown()
  {
    RemoveListener();
    eventBus = null;
  }
  void AddListener()
  {
    eventBus.MyUIEvent?.AddListener(this);
  }
  void RemoveListener()
  {
    if (eventBus != null)
      eventBus.MyUIEvent?.RemoveListener(this);
  }

  public void ShowUI() => canvas.enabled = true;
  public void HideUI() => canvas.enabled = false;

  /** Frequent Updates */
  public void UpdateUnitInfoPanel(UnitController unit)
  {
    UnitNameDisplay.text = $"Lv {unit.CurrentLevel} {unit.UnitName}";
    HPDisplay.UpdateValue(unit.CurrentHP, unit.TotalHP);
    MPDisplay.UpdateValue(unit.CurrentMP, unit.TotalMP);
    APDisplay.UpdateValue(unit.GetCurrentAP(), unit.GetTotalAP());
  }

  public void ClearUnitInfoPanel()
  {
    UnitNameDisplay.text = "Lv ";
    HPDisplay.Reset();
    MPDisplay.Reset();
    APDisplay.Reset();
  }

  public void ShowActions(UnitController unit)
  {
    if (actionSelection.Count == 0)
    {
      foreach (KeyValuePair<string, Skill> action in unit.Actives)
        AddActionSelectionButton(unit, action.Value);
      return;
    }
    if (prevUnit != null && prevUnit.UnitName == unit.UnitName)
    {
      if (prevUnit.CurrentLevel == unit.CurrentLevel) // No change
        return;
      else // Update with new skills only
        foreach (KeyValuePair<string, Skill> action in unit.Actives)
          if (!actionSelectionKeys.Contains(action.Key))
            AddActionSelectionButton(unit, action.Value);
    }
    else // Populate / Re-populate
    {
      foreach (ActionSelect _button in actionSelection)
        DestroyImmediate(_button.gameObject);
      ClearActionSelection();

      foreach (KeyValuePair<string, Skill> action in unit.Actives)
        AddActionSelectionButton(unit, action.Value);

      prevUnit = unit;
    }
  }

  private void AddActionSelectionButton(UnitController unit, Skill skill)
  {
    if (unit.CurrentLevel < skill.LevelReq) return;

    ActionSelect _button = Instantiate(ActionButton, UnitActionsParent).GetComponent<ActionSelect>();
    _button.ButtonLabel.text = skill.Name;
    _button.SetActionDetails(skill.Name, skill.Level, skill.Description);
    _button.MyButton.onClick.RemoveAllListeners();
    _button.MyButton.onClick.AddListener(() =>
    {
      ActionInfoDisplay.text = _button.GetActionInfo();
      ActionDescriptionDisplay.text = _button.GetActionDescription();
      StartCoroutine(unit.PerformAction(skill));
    });
    AddToActionSelection(_button);
  }

  public void ClearActionDescription()
  {
    ActionInfoDisplay.text = "";
    ActionDescriptionDisplay.text = "";
  }

  public void ClearUnitActionsPanel()
  {
    foreach (ActionSelect _button in actionSelection)
      DestroyImmediate(_button.gameObject);
    ClearActionSelection();
  }

  public void AddToUnitSelection(UnitController unit)
  {
    UnitSelect _button = Instantiate(UnitButton, UnitSelectParent).GetComponent<UnitSelect>();
    unit.SetUnitSelectButton(_button);
    _button.BindUnit(unit);
    unitSelection.Add(_button);
    unitSelectionKeys.Add(_button.UnitName);
  }

  public void RemoveFromUnitSelection(UnitController unit)
  {
    unitSelectionKeys.Remove(unit.UnitSelectButton.UnitName);
    unitSelection.Remove(unit.UnitSelectButton);
    unit.UnitSelectButton.UnbindUnit(unit);
    unit.SetUnitSelectButton(null);
  }
  private void AddToActionSelection(ActionSelect _button)
  {
    actionSelection.Add(_button);
    actionSelectionKeys.Add(_button.ActionName);
  }

  private void ClearActionSelection()
  {
    actionSelection.Clear();
    actionSelectionKeys.Clear();
  }

}
