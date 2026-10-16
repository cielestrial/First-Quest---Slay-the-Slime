using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]

[RequireComponent(typeof(RectTransform))]
public class UnitSelect : MonoBehaviour
{
  public Button MyButton { get; private set; }
  public string UnitName { get; private set; }
  public TMP_Text ButtonLabel { get; private set; }
  public UnitController ControlledUnit { get; private set; }
  [SerializeField] private EventBus MyEventBus;

  public
  void Awake()
  {
    MyButton = GetComponent<Button>();
    ButtonLabel = transform.GetChild(0).GetComponent<TMP_Text>();
  }
  public void BindUnit(UnitController unit)
  {
    MyButton.onClick.RemoveAllListeners();
    ControlledUnit = unit;
    SetLabel(unit);
    MyButton.onClick.AddListener(() =>
    {
      MyEventBus.MyTurnEvent.UnitSelected(unit);
      MyEventBus.MyUIEvent.UpdateUnitInfo(unit);
      MyEventBus.MyUIEvent.ShowActions(unit);
      MyEventBus.MyUIEvent.ClearActionDescription();

      MyEventBus.MyTargetEvent.FocusCameraOn(unit);
    });
  }

  private void SetLabel(UnitController unit)
  {
    UnitName = unit.UnitName;
    ButtonLabel.text = unit.UnitName;
  }
}
