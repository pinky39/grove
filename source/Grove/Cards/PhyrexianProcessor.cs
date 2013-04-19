namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Triggers;
  using Core.Zones;

  public class PhyrexianProcessor : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Phyrexian Processor")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text(
          "As Phyrexian Processor enters the battlefield, pay any amount of life.{EOL}{4},{T}: Put an X/X black Minion creature token onto the battlefield, where X is the life paid as Phyrexian Processor entered the battlefield.")
        .OverrideScore(new ScoreOverride {Hand = 50})
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new PayLifeAddCounters();
            p.UsesStack = false;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{4},{T}: Put an X/X black Minion creature token onto the battlefield, where X is the life paid as Phyrexian Processor entered the battlefield.";

            p.Cost = new AggregateCost(
              new PayMana(4.Colorless(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Minion Token")
                .FlavorText("A minion given over to Tevesh Szat is a stronger minion gained.")
                .Type("Creature - Token - Minion")
                .Colors(CardColor.Black),
              
              tokenParameters: (e, token) =>
                {
                  token.Power(e.Source.OwningCard.Counters.GetValueOrDefault());
                  token.Toughness(e.Source.OwningCard.Counters.GetValueOrDefault());
                });

            p.TimingRule(new Any(
              new Steps(passiveTurn: true, activeTurn: false, steps: Step.DeclareAttackers),
              new OwningCardWillBeDestroyed()));
          });
    }
  }
}