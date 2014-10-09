namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class BlackCat : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Black Cat")
          .ManaCost("{1}{B}")
          .Type("Creature — Zombie Cat")
          .Text("When Black Cat dies, target opponent discards a card at random.")
          .FlavorText("Its last life is spent tormenting your dreams.")
          .Power(1)
          .Toughness(1)
          .TriggeredAbility(p =>
          {
            p.Text = "When Black Cat dies, target opponent discards a card at random.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new OpponentDiscardsCards(randomCount: 1);
          });
    }
  }
}
