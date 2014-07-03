namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Effects;
  using Triggers;

  public class ShivanPhoenix : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Shivan Phoenix")
        .ManaCost("{4}{R}{R}")
        .Type("Creature Phoenix")
        .Text("{Flying}{EOL}When Shivan Phoenix dies, return Shivan Phoenix to its owner's hand.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[4])
        .Power(3)
        .Toughness(4)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Shivan Phoenix dies, return Shivan Phoenix to its owner's hand.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}