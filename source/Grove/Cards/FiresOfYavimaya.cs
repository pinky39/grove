namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class FiresOfYavimaya : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Fires of Yavimaya")
        .ManaCost("{1}{R}{G}")
        .Type("Enchantment")
        .Text(
          "Creatures you control have haste.{EOL}Sacrifice Fires of Yavimaya: Target creature gets +2/+2 until end of turn.")
        .Cast(p =>
          {
            p.TimingRule(new FirstMain());
            p.TimingRule(new ThereCanBeOnlyOne());
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddStaticAbility(Static.Haste);
            p.CardFilter = (card, effect) => card.Controller == effect.Source.Controller && card.Is().Creature;
          })
        .ActivatedAbility(p =>
          {
            p.Text = "Sacrifice Fires of Yavimaya: Target creature gets +2/+2 until end of turn.";
            p.Cost = new Sacrifice();
            p.Effect = () => new ApplyModifiersToTargets(() => new AddPowerAndToughness(2, 2) {UntilEot = true})
              {Category = EffectCategories.ToughnessIncrease};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new IncreasePowerOrToughness(2, 2));
          }
        );
    }
  }
}