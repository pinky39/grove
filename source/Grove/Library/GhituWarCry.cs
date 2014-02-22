namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.RepetitionRules;
  using Gameplay.AI.TargetingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class GhituWarCry : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ghitu War Cry")
        .ManaCost("{2}{R}")
        .Type("Enchantment")
        .Text("{R}: Target creature gets +1/+0 until end of turn.")
        .FlavorText("The war cry is not simply a shout but a sacrament.")
        .ActivatedAbility(p =>
          {
            p.Text = "{R}: Target creature gets +1/+0 until end of turn.";
            p.Cost = new PayMana(Mana.Red, ManaUsage.Abilities, supportsRepetitions: true);
            p.Effect = () => new ApplyModifiersToTargets(() => new AddPowerAndToughness(
              1, 0) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectPumpInstant(1, 0));
            p.RepetitionRule(new RepeatMaxTimes());
          });
    }
  }
}