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
  [SerializeField] private EventSystem MyEventSystem;

  private Camera mainCamera;

  public
  void Awake()
  {
    MyButton = GetComponent<Button>();
    ButtonLabel = transform.GetChild(0).GetComponent<TMP_Text>();
    mainCamera = Camera.main;
  }
  public void BindUnit(UnitController unit)
  {
    MyButton.onClick.RemoveAllListeners();
    ControlledUnit = unit;
    SetLabel(unit);
    MyButton.onClick.AddListener(() =>
    {
      MyEventSystem.MyTurnEvent.UnitSelected(unit);
      MyEventSystem.MyUIEvent.UpdateUnitInfo(unit);
      MyEventSystem.MyUIEvent.ShowActions(unit);
      MyEventSystem.MyUIEvent.ClearActionDescription();

      // Focus camera
      mainCamera.transform.position = new Vector3(
        unit.transform.position.x,
        mainCamera.transform.position.y,
        unit.transform.position.z - (25.2f - 9.8f)
      );
    });
  }

  private void SetLabel(UnitController unit)
  {
    UnitName = unit.UnitName;
    ButtonLabel.text = unit.UnitName;
  }
}
