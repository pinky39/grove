namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Triggers;

  public class WallOfJunk : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Wall of Junk")
        .Type("Artifact Creature Wall")
        .ManaCost("{2}")
        .Text(
          "{Defender}{EOL}Whenever Wall of Junk blocks, return it to its owner's hand at end of combat. (Return it only if it's on the battlefield.)")
        .FlavorText(
          "Urza saw the wall and realized that even if he tore every Phyrexian to pieces, they would still resist him.")
        .Power(0)
        .Toughness(7)
        .Timing(Timings.Creatures())
        .Abilities(
          StaticAbility.Defender,
          C.TriggeredAbility(
            "Whenever Wall of Junk blocks, return it to its owner's hand at end of combat. (Return it only if it's on the battlefield.)",
            C.Trigger<AtBegginingOfStep>((t, _) =>
              {
                t.Step = Step.EndOfCombat;
                t.PassiveTurn = true;
                t.ActiveTurn = false;
                t.Condition = self => self.OwningCard.IsBlocker;
              }),
            C.Effect<ReturnToOwnersHand>()
            )
        );
    }
  }
}