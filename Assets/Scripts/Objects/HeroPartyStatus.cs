using UnityEngine;

[CreateAssetMenu(fileName = "HeroPartyStatus", menuName = "Scriptable Objects/HeroPartyStatus")]
public class HeroPartyStatus : ScriptableObject
{
  public bool isPlaying;

  public KnightController Knight { get; private set; }
  public MageController Mage { get; private set; }
  public ArcherController Archer { get; private set; }
  public RogueController Rogue { get; private set; }

  public static readonly int BlockStateHash = Animator.StringToHash("Block");

  public int GetPartyHp()
  {
    int partyHp = 0;
    if (Knight) partyHp += Knight.CurrentHP;
    if (Mage) partyHp += Mage.CurrentHP;
    if (Archer) partyHp += Archer.CurrentHP;
    if (Rogue) partyHp += Rogue.CurrentHP;
    return partyHp;
  }

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

  public void SetKnight(KnightController unit)
  {
    Knight = unit;
    Knight.UnitName = "Knight";
    UIManager.Instance.AddUnitSelectionButton(Knight);
  }

  public void SetMage(MageController unit)
  {
    Mage = unit;
    Mage.UnitName = "Mage";
    UIManager.Instance.AddUnitSelectionButton(Mage);
  }

  public void SetArcher(ArcherController unit)
  {
    Archer = unit;
    Archer.UnitName = "Archer";
    UIManager.Instance.AddUnitSelectionButton(Archer);
  }

  public void SetRogue(RogueController unit)
  {
    Rogue = unit;
    Rogue.UnitName = "Rogue";
    UIManager.Instance.AddUnitSelectionButton(Rogue);
  }

}
