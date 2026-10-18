using Unit.Data;

public abstract class HeroController : UnitController
{
  protected static new int CurrentAP = 2;
  protected static new int TotalAP = 2;
  public override int GetCurrentAP() => CurrentAP;
  public override int GetTotalAP() => TotalAP;

  public override void ConsumeResources(SkillResourceCost cost)
  {
    CurrentAP = ConsumeResource(CurrentAP, cost.AP);
    CurrentMP = ConsumeResource(CurrentMP, cost.MP);
  }
  public override bool SufficientResources(SkillResourceCost cost)
  => SufficientResource(CurrentAP, cost.AP) && SufficientResource(CurrentMP, cost.MP);

  public override void ResetCurrentAP() { CurrentAP = TotalAP; }
  public override void IncreaseTotalAP(int byValue)
  {
    if (byValue > 0)
      TotalAP += byValue;
  }

}
