namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class NoRestForTheWicked : CardTemplateSource
  {
    private static bool WasPutIntoGraveyardThisTurnFromBattlefield(Card card, Game game)
    {
      return card.Is().Creature && game.Turn.Events.HasChangedZone(card, @from: Zone.Battlefield, to: Zone.Graveyard);
    }

    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("No Rest for the Wicked")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text(
          "Sacrifice No Rest for the Wicked: Return to your hand all creature cards in your graveyard that were put there from the battlefield this turn.")
        .FlavorText("The soul? Here, we have no use for such frivolities.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice No Rest for the Wicked: Return to your hand all creature cards in your graveyard that were put there from the battlefield this turn.";
            p.Cost = new Sacrifice();

            p.Effect = () => new ReturnAllCardsInGraveyardToHand(WasPutIntoGraveyardThisTurnFromBattlefield);

            p.TimingRule(new Any(new OnStep(Step.EndOfTurn), new WhenOwningCardWillBeDestroyed()));
            p.TimingRule(new WhenYourGraveyardCountIs(minCount: 1,
              selector: WasPutIntoGraveyardThisTurnFromBattlefield));
          });
    }
  }
}