namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class SatyrWayfinder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Satyr Wayfinder")
        .ManaCost("{1}{G}")
        .Type("Creature - Satyr")
        .Text(
          "When Satyr Wayfinder enters the battlefield, reveal the top four cards of your library. You may put a land card from among them into your hand. Put the rest into your graveyard.")
        .FlavorText("The first satyr to wake after a revel must search for the site of the next one.")
        .Power(1)
        .Toughness(1)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Satyr Wayfinder enters the battlefield, reveal the top four cards of your library. You may put a land card from among them into your hand. Put the rest into your graveyard.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new RevealTopCardsPutOneInHandOthersIntoGraveyard(4, c => c.Is().Land);            
          });
    }
  }
}