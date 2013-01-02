namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;

  public class Attunement : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Attunement")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text("Return Attunement to its owner's hand: Draw three cards, then discard four cards.")
        .FlavorText("The solution can hide for only so long.{EOL}—Urza")
        .Timing(Timings.SecondMain())
        .Abilities(
          ActivatedAbility(
            "Return Attunement to its owner's hand: Draw three cards, then discard four cards.",
            Cost<ReturnToHand>(),
            Effect<DrawCards>(e =>
              {
                e.DrawCount = 3;
                e.DiscardCount = 4;
              }),
            timing: Any(Timings.EndOfTurn(), Timings.CanBeDestroyedByTopSpell())
            )
        );
    }
  }
}