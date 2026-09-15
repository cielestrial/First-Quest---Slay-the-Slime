public class HeroController : UnitController
{
  public static new int CurrentAP = 2;
  public static new int TotalAP = 2;

  public override int GetCurrentAP() => CurrentAP;
  public override int GetTotalAP() => TotalAP;
  public override void SetCurrentAP(int toValue)
  {
    if (toValue < 0)
      CurrentAP = 0;
    else if (toValue > TotalAP)
      CurrentAP = TotalAP;
    else
      CurrentAP = toValue;
  }
  public override void ResetCurrentAP()
  {
    CurrentAP = TotalAP;
  }
  public override void IncreaseTotalAP(int byValue)
  {
    if (byValue > 0)
      TotalAP += byValue;
  }

}
