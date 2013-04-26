namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Zones;

  public class WallOfBlossoms : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Wall of Blossoms")
        .ManaCost("{1}{G}")
        .Type("Creature Plant Wall")
        .Text("{Defender}{EOL}When Wall of Blossoms enters the battlefield, draw a card.")
        .FlavorText("Each flower identical, every leaf and petal disturbingly exact.")
        .Power(0)
        .Toughness(4)
        .StaticAbilities(Static.Defender)
        .TriggeredAbility(p =>
          {
            p.Text = "When Wall of Blossoms enters the battlefield, draw a card.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DrawCards(1);
          });
    }
  }
}