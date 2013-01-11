namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;

  public class WallOfJunk : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Wall of Junk")
        .Type("Artifact Creature Wall")
        .ManaCost("{2}")
        .Text(
          "{Defender}{EOL}Whenever Wall of Junk blocks, return it to its owner's hand at end of combat. (Return it only if it's on the battlefield.)")
        .FlavorText(
          "Urza saw the wall and realized that even if he tore every Phyrexian to pieces, they would still resist him.")
        .Power(0)
        .Toughness(7)
        .Abilities(
          Static.Defender,
          TriggeredAbility(
            "Whenever Wall of Junk blocks, return it to its owner's hand at end of combat. (Return it only if it's on the battlefield.)",
            Trigger<OnStepStart>(t =>
              {
                t.Step = Step.EndOfCombat;
                t.PassiveTurn = true;
                t.ActiveTurn = false;
                t.Condition = self => self.OwningCard.IsBlocker;
              }),
            Effect<ReturnToHand>(e => e.ReturnOwner = true)
            )
        );
    }
  }
}