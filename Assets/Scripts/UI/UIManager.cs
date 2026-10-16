using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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

  public void AddDependencies(EventBus eventBus)
  {
    this.eventBus = eventBus;
  }
  public void ClearDependencies()
  {
    eventBus = null;
  }
  public void AddListener()
  {
    eventBus.MyUIEvent?.AddListener(this);
  }
  public void RemoveListener()
  {
    eventBus.MyUIEvent?.RemoveListener(this);
  }

  public void AddUnitSelectionButton(UnitController unit)
  {
    UnitSelect _button = Instantiate(UnitButton, UnitSelectParent).GetComponent<UnitSelect>();
    unit.SetUnitSelectButton(_button);
    _button.BindUnit(unit);
    AddToUnitSelection(_button);
  }

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
      foreach (KeyValuePair<string, int> action in unit.SkillTree)
        AddActionSelectionButton(unit, action);
      return;
    }
    if (prevUnit != null && prevUnit.UnitName == unit.UnitName)
    {
      if (prevUnit.CurrentLevel == unit.CurrentLevel) // No change
        return;
      else // Update with new skills only
        foreach (KeyValuePair<string, int> action in unit.SkillTree)
          if (!actionSelectionKeys.Contains(action.Key))
            AddActionSelectionButton(unit, action);
    }
    else // Populate / Re-populate
    {
      foreach (ActionSelect _button in actionSelection)
        DestroyImmediate(_button.gameObject);
      ClearActionSelection();

      foreach (KeyValuePair<string, int> action in unit.SkillTree)
        AddActionSelectionButton(unit, action);

      prevUnit = unit;
    }
  }

  private void AddActionSelectionButton(UnitController unit, KeyValuePair<string, int> action)
  {
    if (unit.CurrentLevel < action.Value) return;

    ActionSelect _button = Instantiate(ActionButton, UnitActionsParent).GetComponent<ActionSelect>();
    _button.ButtonLabel.text = action.Key;
    _button.SetActionDetails(action.Key, action.Value, unit.SkillDesc[action.Key]);
    _button.MyButton.onClick.RemoveAllListeners();
    _button.MyButton.onClick.AddListener(() =>
    {
      ActionInfoDisplay.text = _button.GetActionInfo();
      ActionDescriptionDisplay.text = _button.GetActionDescription();
      StartCoroutine(unit.PerformAction(action.Key));
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

  private void AddToUnitSelection(UnitSelect _button)
  {
    unitSelection.Add(_button);
    unitSelectionKeys.Add(_button.UnitName);
  }

  public void RemoveUnitSelection(UnitController unit)
  {
    unitSelection.Remove(unit.UnitSelectButton);
    unitSelectionKeys.Remove(unit.UnitSelectButton.UnitName);
    DestroyImmediate(unit.gameObject);
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
