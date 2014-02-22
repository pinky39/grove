namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;

  public class PhyrexianProcessor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phyrexian Processor")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text(
          "As Phyrexian Processor enters the battlefield, pay any amount of life.{EOL}{4},{T}: Put an X/X black Minion creature token onto the battlefield, where X is the life paid as Phyrexian Processor entered the battlefield.")
        .OverrideScore(new ScoreOverride {Hand = 50})
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new PayLifeAddCounters(CounterType.Fake);
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
                  token.Power(e.Source.OwningCard.Counters);
                  token.Toughness(e.Source.OwningCard.Counters);
                });

            p.TimingRule(new Any(
              new AfterOpponentDeclaresAttackers(),
              new WhenOwningCardWillBeDestroyed(),
              new OnEndOfOpponentsTurn()));
          });
    }
  }
}