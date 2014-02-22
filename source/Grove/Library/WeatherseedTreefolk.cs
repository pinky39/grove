namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;

  public class WeatherseedTreefolk : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}