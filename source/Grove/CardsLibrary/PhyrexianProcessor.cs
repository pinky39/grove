namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Triggers;

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
        .OverrideScore(p => p.Hand = 50)
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
              new PayMana(4.Colorless()),
              new Tap());

            p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Minion")
                .FlavorText("A minion given over to Tevesh Szat is a stronger minion gained.")
                .Type("Token Creature - Minion")
                .Colors(CardColor.Black),
              tokenParameters: (token, ctx) =>
                {
                  token.Power(ctx.OwningCard.Counters);
                  token.Toughness(ctx.OwningCard.Counters);
                });

            p.TimingRule(new Any(
              new AfterOpponentDeclaresAttackers(),
              new WhenOwningCardWillBeDestroyed(),
              new OnEndOfOpponentsTurn()));
          });
    }
  }
}