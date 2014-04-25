namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class SkitteringHorror : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Skittering Horror")
        .ManaCost("{2}{B}")
        .Type("Creature Horror")
        .Text("When you cast a creature spell, sacrifice Skittering Horror.")
        .FlavorText(
          "This monstrosity will do—for now.")
        .Power(4)
        .Toughness(3)        
        .TriggeredAbility(p =>
          {
            p.Text = "When you cast a creature spell, sacrifice Skittering Horror.";
            p.Trigger(new OnCastedSpell((a, c) =>
              a.OwningCard.Controller == c.Controller && c.Is().Creature));

            p.Effect = () => new SacrificeOwner();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}