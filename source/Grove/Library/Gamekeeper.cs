namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;

  public class Gamekeeper : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Gamekeeper")
        .ManaCost("{3}{G}")
        .Type("Creature Elf")
        .Text(
          "When Gamekeeper dies, you may exile it. If you do, reveal cards from the top of your library until you reveal a creature card. Put that card onto the battlefield and put all other cards revealed this way into your graveyard.")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Gamekeeper dies, you may exile it. If you do, reveal cards from the top of your library until you reveal a creature card. Put that card onto the battlefield and put all other cards revealed this way into your graveyard.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));

            p.Effect = () => new CompoundEffect(
              new ExileOwner(),
              new PutFirstCreatureInPlayPutOtherCardsInGraveyard());
          });
    }
  }
}