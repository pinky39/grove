namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class WeatherseedTreefolk : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Weatherseed Treefolk")
        .ManaCost("{2}{G}{G}{G}")
        .Type("Creature Treefolk")
        .Text("{Trample}{EOL}When Weatherseed Treefolk dies, return it to its owner's hand.")
        .Power(5)
        .Toughness(3)
        .SimpleAbilities(Static.Trample)
        .TriggeredAbility(p =>
          {
            p.Text = "When Weatherseed Treefolk dies, return it to its owner's hand.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}