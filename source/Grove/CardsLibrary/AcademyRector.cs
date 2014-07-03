namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Effects;
  using Triggers;

  public class AcademyRector : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Academy Rector")
        .ManaCost("{3}{W}")
        .Type("Creature Human Cleric")
        .Text(
          "When Academy Rector dies, you may exile it. If you do, search your library for an enchantment card, put that card onto the battlefield, then shuffle your library.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
        .Power(1)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Academy Rector dies, you may exile it. If you do, search your library for an enchantment card, put that card onto the battlefield, then shuffle your library.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));

            p.Effect = () => new CompoundEffect(
              new ExileOwner(),
              new SearchLibraryPutToZone(
                zone: Zone.Battlefield,
                minCount: 0,
                maxCount: 1,
                validator: (e, c) => c.Is().Enchantment,
                text: "Search your library for an enchantment."));
          });
    }
  }
}