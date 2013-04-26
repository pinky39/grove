namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.States;
  using Gameplay.Zones;

  public class NoRestForTheWicked : CardsSource
  {
    private static bool WasPutIntoGraveyardThisTurnFromBattlefield(Card card)
    {
      return card.Is().Creature && card.HasChangedZoneThisTurn && card.PreviousZone == Zone.Battlefield;
    }

    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("No Rest for the Wicked")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text(
          "Sacrifice No Rest for the Wicked: Return to your hand all creature cards in your graveyard that were put there from the battlefield this turn.")
        .FlavorText("The soul? Here, we have no use for such frivolities.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice No Rest for the Wicked: Return to your hand all creature cards in your graveyard that were put there from the battlefield this turn.";
            p.Cost = new Sacrifice();
            p.Effect = () => new ReturnAllCardsInGraveyardToHand(WasPutIntoGraveyardThisTurnFromBattlefield);
            p.TimingRule(new Any(new Steps(Step.EndOfTurn), new OwningCardWillBeDestroyed()));
            p.TimingRule(new ControllerGravayardCountIs(minCount: 1,
              selector: WasPutIntoGraveyardThisTurnFromBattlefield));
          });
    }
  }
}