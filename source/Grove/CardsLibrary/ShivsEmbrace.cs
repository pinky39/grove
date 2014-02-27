namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.RepetitionRules;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class ShivsEmbrace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Shiv's Embrace")
        .ManaCost("{2}{R}{R}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchanted creature gets +2/+2 and has flying.{EOL}{R}: Enchanted creature gets +1/+0 until end of turn.")
        .FlavorText("Wear the foe's form to best it in battle. So sayeth the bey.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () =>
                {
                  var ap = new ActivatedAbility.Parameters
                    {
                      Text = "{R}: Enchanted creature gets +1/+0 until end of turn.",
                      Cost = new PayMana(Mana.Red, ManaUsage.Abilities, supportsRepetitions: true),
                      Effect = () => new ApplyModifiersToSelf(
                        () => new AddPowerAndToughness(1, 0) {UntilEot = true}).SetTags(EffectTag.IncreasePower)
                    };

                  ap.TimingRule(new PumpOwningCardTimingRule(1, 0));
                  ap.RepetitionRule(new RepeatMaxTimes());
                  return new AddActivatedAbility(new ActivatedAbility(ap));
                },
              () => new AddPowerAndToughness(2, 2),
              () => new AddStaticAbility(Static.Flying)).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          });
    }
  }
}