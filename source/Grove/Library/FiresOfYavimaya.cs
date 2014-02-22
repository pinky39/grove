namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class FiresOfYavimaya : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fires of Yavimaya")
        .ManaCost("{1}{R}{G}")
        .Type("Enchantment")
        .Text(
          "Creatures you control have haste.{EOL}Sacrifice Fires of Yavimaya: Target creature gets +2/+2 until end of turn.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenYouDontControlSamePermanent());
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
              .SetTags(EffectTag.IncreaseToughness, EffectTag.IncreasePower);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectPumpInstant(2, 2));
          }
        );
    }
  }
}