namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Counters;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;

  public class VampiricEmbrace : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Vampiric Embrace")
        .ManaCost("{2}{B}{B}")
        .Type("Enchantment Aura")
        .Text(
          "Enchanted creature gets +2/+2 and has flying.{EOL}Whenever a creature dealt damage by enchanted creature this turn dies, put a +1/+1 counter on that creature.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(2, 2),
              () => new AddStaticAbility(Static.Flying),
              () =>
                {
                  var tp = new TriggeredAbilityParameters
                    {
                      Text = "Whenever a creature dealt damage by enchanted creature this turn dies, put a +1/+1 counter on that creature.",
                      Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 1))
                    };

                  tp.Trigger(new OnCreatureDamagedByOwnerWasPutToGraveyard());                  
                  
                  return new AddTriggeredAbility(new TriggeredAbility(tp));
                }) {Category = EffectCategories.ToughnessIncrease};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
          });
    }
  }
}