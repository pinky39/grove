namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class NecromancersAssistant : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Necromancer's Assistant")
        .ManaCost("{2}{B}")
        .Type("Creature — Zombie")
        .Text("When Necromancer's Assistant enters the battlefield, put the top three cards of your library into your graveyard.")
        .FlavorText("Zombies and necromancers agree: easy access to brains is preferred.")
        .Power(3)
        .Toughness(1)
        .TriggeredAbility(p =>
        {
          p.Text = "When Necromancer's Assistant enters the battlefield, put the top three cards of your library into your graveyard.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new PlayerPutsTopCardsFromLibraryToGraveyard(P(e => e.Controller), count: 3);
        });
    }
  }
}
