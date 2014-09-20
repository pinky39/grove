namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class TelepathicSpies : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Telepathic Spies")
        .ManaCost("{2}{U}")
        .Type("Creature Human Wizard")
        .Text("When Telepathic Spies enters the battlefield, look at target opponent's hand.")
        .FlavorText("Do try to keep an open mind, would you?")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text = "When Telepathic Spies enters the battlefield, look at target opponent's hand.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new OpponentRevealsHand();          
          });
    }
  }
}