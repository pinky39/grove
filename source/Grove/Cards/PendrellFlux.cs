namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class PendrellFlux : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
                    TriggerOnlyIfOwningCardIsInPlay = true
                  };

                tp.Trigger(new OnStepStart(Step.Upkeep));

                return new AddTriggeredAbility(new TriggeredAbility(tp));
              });

            p.TimingRule(new SecondMain());
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new OrderByRank(c => -c.Score, ControlledBy.Opponent));
          });
    }
  }
}