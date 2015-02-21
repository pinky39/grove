namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

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
                var tp = new TriggeredAbility.Parameters
                  {
                    Text = "At the beginning of your upkeep, sacrifice this creature unless you pay its mana cost",
                    Effect =
                      () => new PayManaThen(P(e => e.Source.OwningCard.ManaCost), 
                        effect: new SacrificeOwner(),
                        parameters: new PayThen.Parameters()
                        {
                          ExecuteIfPaid = false,
                          Message = "Pay creature's mana cost?"
                        })
                  };

                tp.Trigger(new OnStepStart(Step.Upkeep));

                return new AddTriggeredAbility(new TriggeredAbility(tp));
              });

            p.TimingRule(new OnSecondMain());
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score, ControlledBy.Opponent));
          });
    }
  }
}