using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionSelect : MonoBehaviour
{
  public Button Button { get; private set; }
  public TMP_Text ButtonLabel { get; private set; }
  public string ActionName { get; private set; }
  private string ActionInfo;
  private string ActionDescription;

  void Awake()
  {
    Button = GetComponent<Button>();
    ButtonLabel = transform.GetChild(0).GetComponent<TMP_Text>();
  }

  public void SetActionDetails(string name, int lv, string description)
  {
    ActionName = name;
    string[] decscriptionSegments = description.Split(": ");
    ActionInfo = $"Lv {lv} {name}\n{decscriptionSegments[0]}";
    ActionDescription = decscriptionSegments[1];
  }

  public string GetActionInfo() => ActionInfo;
  public string GetActionDescription() => ActionDescription;

}
