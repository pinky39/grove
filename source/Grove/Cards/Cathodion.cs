namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Zones;

  public class Cathodion : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Cathodion")
        .ManaCost("{3}")
        .Type("Artifact Creature Construct")
        .Text("When Cathodion dies, add {3} to your mana pool.")
        .FlavorText(
          "Instead of creating a tool that would be damaged by heat, the Thran built one that was charged by it.")
        .Power(3)
        .Toughness(3)        
        .TriggeredAbility(p =>
          {
            p.Text = "When Cathodion dies, add {3} to your mana pool.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new AddManaToPool(3.Colorless());
          }            
        );
    }
  }
}