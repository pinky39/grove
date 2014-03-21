namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Triggers;

  public class EmperorCrocodile : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Emperor Crocodile")
        .ManaCost("{3}{G}")
        .Type("Creature Crocodile")
        .Text("When you control no other creatures, sacrifice Emperor Crocodile.")
        .FlavorText("The king of Yavimaya's waters pays constant attention to his subjects . . . and thrives on their adulation.")
        .Power(5)
        .Toughness(5)        
        .TriggeredAbility(p =>
          {
            p.Text = "When you control no other creatures, sacrifice Emperor Crocodile.";
            p.Trigger(new OnEffectResolved(
              filter: (ability, game) => ability.OwningCard.Controller
                .Battlefield.Creatures.Count() <= 1));

            p.Effect = () => new SacrificeOwner();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}