namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class HighlandGame : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Highland Game")
          .ManaCost("{1}{G}")
          .Type("Creature — Elk")
          .Text("When Highland Game dies, you gain 2 life.")
          .FlavorText("\"Bring down a stag and fix its horns upon her head. This one hears the whispers.\"{EOL}—Chianul, at the weaving of Arel")
          .Power(2)
          .Toughness(1)
          .TriggeredAbility(p =>
          {
            p.Text = "When Highland Game dies, you gain 2 life.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ChangeLife(2, whos: P(e => e.Controller));
          });
    }
  }
}
