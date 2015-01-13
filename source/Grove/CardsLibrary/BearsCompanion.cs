namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class BearsCompanion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bear's Companion")
        .ManaCost("{2}{G}{U}{R}")
        .Type("Creature - Human Warrior")
        .Text("When Bear's Companion enters the battlefield, put a 4/4 green Bear creature token onto the battlefield.")
        .FlavorText("\"The Sultai came hunting for a bear hide. Now I have a belt of naga skin, and my friend has a full belly.\"")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
        {
          p.Text = "When Bear's Companion enters the battlefield, put a 4/4 green Bear creature token onto the battlefield.";

          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

          p.Effect = () => new CreateTokens(
            count: 1,
            token: Card
              .Named("Bear")
              .Power(4)
              .Toughness(4)
              .Type("Token Creature - Bear")
              .Colors(CardColor.Green));
        });
    }
  }
}
