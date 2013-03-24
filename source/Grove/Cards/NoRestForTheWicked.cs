namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;

  public class NoRestForTheWicked : CardsSource
  {
    // todo add previous zone info, so only creatures the were put to graveyard from battlefield get returned.
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("No Rest for the Wicked")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text("Sacrifice No Rest for the Wicked: Return to your hand all creature cards in your graveyard that were put there from the battlefield this turn.")
        .FlavorText("'The soul? Here, we have no use for such frivolities.'{EOL}—Sitrik, birth priest")
        .Cast(p => p.TimingRule(new SecondMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "Sacrifice No Rest for the Wicked: Return to your hand all creature cards in your graveyard that were put there from the battlefield this turn.";
            p.Cost = new Sacrifice();
            p.Effect = () => new ReturnAllCardsInGraveyardToHand(c => c.Is().Creature && c.HasChangedZoneThisTurn);                        
            p.TimingRule(new Steps(Step.EndOfTurn));
            p.TimingRule(new ControllerGravayardCountIs(minCount: 1, selector: c => c.Is().Creature && c.HasChangedZoneThisTurn));
          });
    }
  }
}