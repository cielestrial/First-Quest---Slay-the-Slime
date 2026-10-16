using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionSelect : MonoBehaviour
{
  public Button MyButton { get; private set; }
  public TMP_Text ButtonLabel { get; private set; }
  public string ActionName { get; private set; }
  private string ActionInfo;
  private string ActionDescription;

  void Awake()
  {
    MyButton = GetComponent<Button>();
    ButtonLabel = transform.GetChild(0).GetComponent<TMP_Text>();
  }

  public void SetActionDetails(string name, int lv, string description)
  {
    ActionName = name;
    string[] descriptionSegments = description.Split(": ");
    ActionInfo = $"Lv {lv} {name}\n{descriptionSegments[0]}";
    ActionDescription = descriptionSegments[1];
  }

  public string GetActionInfo() => ActionInfo;
  public string GetActionDescription() => ActionDescription;

}
