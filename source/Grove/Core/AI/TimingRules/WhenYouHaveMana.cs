namespace Grove.AI.TimingRules
{
  public class WhenYouHaveMana : TimingRule
  {
    private readonly int _converted;

    private WhenYouHaveMana() {}

    public WhenYouHaveMana(int converted)
    {
      _converted = converted;
    }

    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return p.Controller.HasMana(_converted);
    }
  }
}