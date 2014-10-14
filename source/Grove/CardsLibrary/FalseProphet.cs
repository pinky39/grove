namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Effects;
  using Triggers;

  public class FalseProphet : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("False Prophet")
        .ManaCost("{2}{W}{W}")
        .Type("Creature Human Cleric")
        .Text("When False Prophet dies, exile all creatures.")
        .FlavorText("You lived for Serra's love. Will you not die for it?")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
        .Power(2)
        .Toughness(2)        
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.Graveyard));

            p.Effect = () => new ExileAllCards(filter: (e, c) => c.Is().Creature);
          });
    }
  }
}