namespace Grove.Core.Ai.TimingRules
{
  using System;
  using System.Linq;
  using Infrastructure;
  using Mana;

  public class ControllerNeedsAdditionalMana : TimingRule
  {
    private readonly int _amount;

    private ControllerNeedsAdditionalMana() {}

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