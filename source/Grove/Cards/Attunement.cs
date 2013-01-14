namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;

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
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          ActivatedAbility(
            "Return Attunement to its owner's hand: Draw three cards, then discard four cards.",
            Cost<Core.Costs.ReturnToHand>(),
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