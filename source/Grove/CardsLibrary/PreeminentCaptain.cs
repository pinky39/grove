namespace Grove.CardsLibrary
{
  using System.Collections.Generic;  
  using Effects;
  using Triggers;

  public class PreeminentCaptain : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Preeminent Captain")
        .ManaCost("{2}{W}")
        .Type("Creature — Kithkin Soldier")
        .Text("{First strike} {I}(This creature deals combat damage before creatures without first strike.){/I}{EOL}Whenever Preeminent Captain attacks, you may put a Soldier creature card from your hand onto the battlefield tapped and attacking.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.FirstStrike)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Preeminent Captain attacks, you may put a Soldier creature card from your hand onto the battlefield tapped and attacking.";

          p.Trigger(new WhenThisAttacks());

          p.Effect = () => new PutSelectedCardsToBattlefield(
            fromZone: Zone.Hand,
            validator: c => c.Is().Creature && c.Is("soldier"), 
            text: "Select a Soldier creature card in your hand.", 
            after: (card, game) => game.Combat.AddAttacker(card));
        });
    }
  }
}
