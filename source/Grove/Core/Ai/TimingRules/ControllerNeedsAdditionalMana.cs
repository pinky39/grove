namespace Grove.Core.Ai.TimingRules
{
  using System.Linq;

  public class ControllerNeedsAdditionalMana : TimingRule
  {
    private readonly int _amount;
    
    public ControllerNeedsAdditionalMana(int amount)
    {
      _amount = amount;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (Turn.Step != Step.FirstMain && Turn.Step != Step.SecondMain)
        return false;

      var availableMana = p.Controller.GetConvertedMana();

      return p.Controller.Hand.Any(x => x.ConvertedCost > availableMana &&
        x.ConvertedCost <= availableMana + _amount);
    }
  }
}