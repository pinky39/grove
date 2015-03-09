namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Grove.Effects;
  using Grove.Triggers;

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
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Gamekeeper dies, you may exile it. If you do, reveal cards from the top of your library until you reveal a creature card. Put that card onto the battlefield and put all other cards revealed this way into your graveyard.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));

            p.Effect = () => new CompoundEffect(
              new ExileOwner(),
              new PutFirstCardInPlayPutOtherCardsToZone(Zone.Graveyard, 
                filter: c => c.Is().Creature));
          });
    }
  }
}