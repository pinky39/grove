namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;

  public class ImaginaryPet : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Imaginary Pet")
        .ManaCost("{1}{U}")
        .Type("Creature Illusion")
        .Text("At the beginning of your upkeep, if you have a card in hand, return Imaginary Pet to its owner's hand.")
        .FlavorText("'It followed me home. Can I keep it?'")
        .Power(4)
        .Toughness(4)
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, if you have a card in hand, return Imaginary Pet to its owner's hand.",
            Trigger<OnStepStart>(t =>
              {
                t.Step = Step.Upkeep;
                t.Condition = self => self.OwningCard.Controller.Hand.Count > 0;
              }),
            Effect<ReturnToHand>(e =>
              {
                e.ReturnOwner = true;
                e.BeforeResolve = self => self.Controller.Hand.Count > 0;
              })
            )
        );
    }
  }
}