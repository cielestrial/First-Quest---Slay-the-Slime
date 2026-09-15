using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsDisplay : MonoBehaviour
{
  [SerializeField] private string StatName;
  public int StatValue { get; private set; }
  public Slider Slider { get; private set; }
  public TMP_Text Text { get; private set; }
  private float visualOffset;

  void Awake()
  {
    // StatName = StatName.ToUpper();
    Text = transform.GetChild(0).GetComponent<TMP_Text>();
    Slider = transform.GetChild(1).GetComponent<Slider>();
  }

  public string GetStatName() => StatName;

  public void UpdateValue(int _value, int _total)
  {
    StatValue = _value;
    Text.text = $"{StatName} {StatValue}";
    if (_value > 0)
    {
      visualOffset = 15f * (_total - _value) / _total;
      Slider.value = Mathf.Ceil(_value * 100f / _total + visualOffset);
    }
    else
      Slider.value = 0;
  }

  public void Reset()
  {
    Text.text = StatName;
    Slider.value = 0;
  }
}
