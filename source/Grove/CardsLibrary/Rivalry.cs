namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class Rivalry : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rivalry")
        .ManaCost("{2}{R}")
        .Type("Enchantment")
        .Text(
          "At the beginning of each player's upkeep, if that player controls more lands than each other player, Rivalry deals 2 damage to him or her.")
        .FlavorText("The goblins revered it; the viashino defended it. Only Urza understood it.")
        .Cast(p =>
          {
            p.TimingRule(new OnSecondMain());
            p.TimingRule(new WhenYouHaveMorePermanents(c => c.Is().Land));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of each player's upkeep, if that player controls more lands than each other player, Rivalry deals 2 damage to him or her.";

            p.Trigger(new OnStepStart(Step.Upkeep, activeTurn: true, passiveTurn: true)
              {
                Condition = ctx =>
                  {
                    var activeCount = ctx.Players.Active.Battlefield.Lands.Count();
                    var passiveCount = ctx.Players.Passive.Battlefield.Lands.Count();
                    return activeCount > passiveCount;
                  }
              });

            p.Effect = () => new DealDamageToPlayer(2, P((e, g) => g.Players.Active));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}