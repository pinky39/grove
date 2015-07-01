namespace Grove.AI.TimingRules
{
  public class ConvertManaTimingRule : TimingRule
  {
    private readonly int _relativeCost;

    private ConvertManaTimingRule() {}

    public ConvertManaTimingRule(int relativeCost)
    {
      _relativeCost = relativeCost;
    }

    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      // currently somehow limited because of performance reasons      
      if (Turn.Step != Step.FirstMain && Turn.Step != Step.SecondMain)
        return false;

      var availableMana = p.Controller.GetAvailableManaCount() - _relativeCost;

      // only cards in hand
      foreach (var card in p.Controller.Hand)
      {
        if (card.ManaCost.Converted <= availableMana && !p.Controller.HasMana(card.ManaCost))
        {
          return true;
        }
      }

      return false;
    }
  }
}