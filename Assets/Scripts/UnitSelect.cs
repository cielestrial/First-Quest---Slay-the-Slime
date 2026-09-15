using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]

[RequireComponent(typeof(RectTransform))]
public class UnitSelect : MonoBehaviour
{
  public Button Button { get; private set; }
  public string UnitName { get; private set; }
  public TMP_Text ButtonLabel { get; private set; }
  public UnitController ControlledUnit { get; private set; }

  private Camera mainCamera;

  public
  void Awake()
  {
    Button = GetComponent<Button>();
    ButtonLabel = transform.GetChild(0).GetComponent<TMP_Text>();
    mainCamera = Camera.main;
  }
  public void BindUnit(UnitController unit)
  {
    ControlledUnit = unit;
    SetLabel();
    Button.onClick.RemoveAllListeners();
    Button.onClick.AddListener(() =>
    {
      TurnManager.Instance.SelectedUnit = unit;
      UIManager.Instance.UpdateUnitInfoPanel(unit);
      UIManager.Instance.ShowActions(unit);
      UIManager.Instance.ClearActionDescription();

      // Focus camera
      mainCamera.transform.position = new Vector3(
        unit.transform.position.x,
        mainCamera.transform.position.y,
        unit.transform.position.z - (25.2f - 9.8f)
      );
      unit.SetTurn();
    });
  }

  private void SetLabel()
  {
    UnitName = ControlledUnit.UnitName;
    ButtonLabel.text = ControlledUnit.UnitName;
  }
}
