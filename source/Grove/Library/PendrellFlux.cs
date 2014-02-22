namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;
  using Grove.Gameplay.Triggers;

  public class PendrellFlux : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Pendrell Flux")
        .ManaCost("{1}{U}")
        .Type("Enchantment Aura")
        .Text(
          "Enchanted creature has 'At the beginning of your upkeep, sacrifice this creature unless you pay its mana cost'")
        .FlavorText("Devoured by the mists, Tolaria was stuck in time, trapped between two eternal heartbeats.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() =>
              {
                var tp = new TriggeredAbilityParameters
                  {
                    Text = "At the beginning of your upkeep, sacrifice this creature unless you pay its mana cost",
                    Effect =
                      () => new PayManaOrSacrifice(P(e => e.Source.OwningCard.ManaCost), "Pay creatures mana cost?"),                    
                  };

                tp.Trigger(new OnStepStart(Step.Upkeep));

                return new AddTriggeredAbility(new TriggeredAbility(tp));
              });

            p.TimingRule(new OnSecondMain());
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectRankBy(c => -c.Score, ControlledBy.Opponent));
          });
    }
  }
}