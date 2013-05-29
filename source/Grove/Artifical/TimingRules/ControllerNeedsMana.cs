namespace Grove.Artifical.TimingRules
{
  using System.Linq;
  using Gameplay.States;

  public class ControllerNeedsAdditionalMana : TimingRule
  {
    private readonly int? _amount;

    private ControllerNeedsAdditionalMana() {}

    public ControllerNeedsAdditionalMana(int? amount = null)
    {
      _amount = amount;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (Turn.Step != Step.FirstMain && Turn.Step != Step.SecondMain)
        return false;

      var availableMana = p.Controller.GetConvertedMana();

      return p.Controller.Hand.Any(x =>
        {
          if (x.ConvertedCost <= availableMana)
            return false;

          if (_amount == null)
            return true;

          return x.ConvertedCost <= availableMana + _amount;
        });
    }
  }
}