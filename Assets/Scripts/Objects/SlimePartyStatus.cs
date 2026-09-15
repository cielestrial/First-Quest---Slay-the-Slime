using UnityEngine;

[CreateAssetMenu(fileName = "SlimePartyStatus", menuName = "Scriptable Objects/SlimePartyStatus")]
public class SlimePartyStatus : ScriptableObject
{
  public bool isPlaying;
  public SlimeController SlimeInstance { get; private set; }
  public int Slime1HP = 100;
  public int Slime2HP = 100;

  public int GetPartyHp() => Slime1HP + Slime2HP;

  public Vector2Int GetStartLocation()
  {
    int startOffset = GridManager.DefaultRowSize * 2;
    int x = Mathf.FloorToInt(GridManager.MaxRowSize / 2);
    if (isPlaying) return new Vector2Int(x, startOffset);
    else return new Vector2Int(x, GridManager.MaxRowSize - 1 - startOffset);
  }

  public Quaternion GetStartQuaternion()
  {
    if (isPlaying) return Quaternion.identity;
    else return Quaternion.Euler(0, 180, 0);
  }

  public void SetSlime(SlimeController unit)
  {
    SlimeInstance = unit;
  }
}
