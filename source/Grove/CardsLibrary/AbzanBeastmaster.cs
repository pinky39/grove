namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Triggers;

  public class AbzanBeastmaster : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Abzan Beastmaster")
        .ManaCost("{2}{G}")
        .Type("Creature - Hound Shaman")
        .Text("At the beginning of your upkeep, draw a card if you control the creature with the greatest toughness or tied for the greatest toughness.")
        .FlavorText("His beasts move the great siege towers of the Abzan across the endless sands.")
        .Power(2)
        .Toughness(1)
        .TriggeredAbility(p =>
        {
          p.Text = "At the beginning of your upkeep, draw a card if you control the creature with the greatest toughness or tied for the greatest toughness.";
          p.Trigger(new OnStepStart(Step.Upkeep)
          {
            Condition = ctx =>
            {
              var creatures = ctx.Players.Permanents().Where(c => c.Is().Creature);

              return ctx.You.Battlefield.Creatures.Any(
                c => creatures.All(x => x.Toughness <= c.Toughness));
            }
          });
          p.Effect = () => new DrawCards(1);
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
