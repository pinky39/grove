namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Triggers;

  public class BrinkOfMadness : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Brink of Madness")
        .ManaCost("{2}{B}{B}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, if you have no cards in hand, sacrifice Brink of Madness and target opponent discards his or her hand.")
        .FlavorText("The fools thought me dead. But I built an empire inside my tomb.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, if you have no cards in hand, sacrifice Brink of Madness and target opponent discards his or her hand.";

            p.Trigger(new OnStepStart(activeTurn: true, passiveTurn: false, step: Step.Upkeep)
              {Condition = (t, g) => t.Controller.Hand.Count == 0});

            p.Effect = () => new CompoundEffect(
              new SacrificeOwner(),
              new OpponentDiscardsHand());
          });
    }
  }
}