namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;

  public class GreenerPastures : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Greener Pastures")
        .ManaCost("{2}{G}")
        .Type("Enchantment")
        .Text(
          "At the beginning of each player's upkeep, if that player controls more lands than each other player, the player puts a 1/1 green Saproling creature token onto the battlefield.")
        .Cast(p => p.Timing = All(Timings.SecondMain(), Timings.HasMorePermanents(x => x.Is().Land)))          
        .Abilities(
          TriggeredAbility(
            "At the beginning of each player's upkeep, if that player controls more lands than each other player, the player puts a 1/1 green Saproling creature token onto the battlefield.",
            Trigger<AtBegginingOfStep>(t =>
              {
                t.Condition = tr =>
                  {
                    var activeCount = tr.Game.Players.Active.Battlefield.Lands.Count();
                    var passiveCount = tr.Game.Players.Passive.Battlefield.Lands.Count();

                    return activeCount > passiveCount;
                  };
                t.Step = Step.Upkeep;
                t.PassiveTurn = true;
                t.ActiveTurn = true;
              }),
            Effect<CreateTokens>(e =>
              {
                e.TokenController = e.Players.Active;
                e.Tokens(
                  Card
                    .Named("Saproling Token")
                    .FlavorText(
                      "The nauseating wriggling of a saproling is exceeded only by the nauseating wriggling of its prey.")
                    .Power(1)
                    .Toughness(1)
                    .Type("Creature - Token - Saproling")
                    .Colors(ManaColors.Green));
              }),
            triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}