namespace Grove.AI.TimingRules
{
  using System.Linq;

  public class WhenYouNeedAdditionalMana : TimingRule
  {
    private readonly int? _amount;

    private WhenYouNeedAdditionalMana() {}

    public WhenYouNeedAdditionalMana(int? amount = null)
    {
      _amount = amount;
    }

    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      if (!Stack.IsEmpty)
        return false;

      if (!p.Controller.IsActive)
        return false;
      
      if (Turn.Step != Step.SecondMain)
        return false;

      var availableMana = p.Controller.GetAvailableConvertedMana(
        canUseConvoke: p.Card.Has().Convoke, 
        canUseDelve: p.Card.Has().Delve);

      return SpellsNeedMana(p, availableMana) || AbilitiesNeedMana(p, availableMana);
    }

    private bool SpellsNeedMana(TimingRuleParameters p, int availableMana)
    {
      return p.Controller.Hand.Any(x =>
        {
          if (x.ConvertedCost <= availableMana)
            return false;

          if (_amount == null)
            return true;

          return x.ConvertedCost <= availableMana + _amount;
        });
    }

    private bool AbilitiesNeedMana(TimingRuleParameters p, int availableMana)
    {
      return p.Controller.Battlefield.Any(x =>
        {
          var manaCosts = x.GetActivatedAbilitiesManaCost();

          foreach (var manaCost in manaCosts)
          {
            if (manaCost.Converted <= availableMana)
              continue;

            if (_amount == null)
              return true;

            if (manaCost.Converted <= availableMana + _amount)
              return true;
          }

          return false;
        });
    }
  }
}