namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class PhyrexianDefiler : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phyrexian Defiler")
        .ManaCost("{2}{B}{B}")
        .Type("Creature Carrier")
        .Text("{T}, Sacrifice Phyrexian Defiler: Target creature gets -3/-3 until end of turn.")
        .FlavorText("The third stage of the illness: muscle aches and persistent cough.")
        .Power(3)
        .Toughness(3)       
        .ActivatedAbility(p =>
          {
            p.Text = "{T}, Sacrifice Phyrexian Defiler: Target creature gets -3/-3 until end of turn.";
            p.Cost = new AggregateCost(
              new Tap(),
              new Sacrifice());

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(-3, -3) { UntilEot = true }) { ToughnessReduction = 3 };

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectReduceToughness(3));
            p.TimingRule(new Any(new WhenOwningCardWillBeDestroyed(), new TargetRemovalTimingRule(combatOnly: true)));
          });
    }
  }
}