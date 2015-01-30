namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class KheruBloodsucker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Kheru Bloodsucker")
          .ManaCost("{2}{B}")
          .Type("Creature — Vampire")
          .Text("Whenever a creature you control with toughness 4 or greater dies, each opponent loses 2 life and you gain 2 life.{EOL}{2}{B}, Sacrifice another creature: Put a +1/+1 counter on Kheru Bloodsucker.")
          .FlavorText("It stares through the empty, pain-twisted faces of those it has drained.")
          .Power(2)
          .Toughness(2)
          .TriggeredAbility(p =>
          {
            p.Text = "Whenever a creature you control with toughness 4 or greater dies, each opponent loses 2 life and you gain 2 life.";
            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield, 
              to: Zone.Graveyard, 
              filter: (card, ability, _) => card.Controller == ability.SourceCard.Controller && card.Is().Creature && card.Toughness >= 4));
            p.Effect = () => new CompoundEffect(
              new ChangeLife(2, yours: true),
              new ChangeLife(-2, opponents: true));
          });
    }
  }
}
