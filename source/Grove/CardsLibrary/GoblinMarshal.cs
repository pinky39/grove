namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Effects;
  using Triggers;

  public class GoblinMarshal : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Marshal")
        .ManaCost("{4}{R}{R}")
        .Type("Creature Goblin Warrior")
        .Text(
          "{Echo} {4}{R}{R}{EOL}When Goblin Marshal enters the battlefield or dies, put two 1/1 red Goblin creature tokens onto the battlefield.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[3])
        .Power(3)
        .Toughness(3)
        .Echo("{4}{R}{R}")
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Goblin Marshal enters the battlefield or dies, put two 1/1 red Goblin creature tokens onto the battlefield.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.Graveyard));

            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield));

            p.Effect = () => new CreateTokens(count: 2,
              token: Card
                .Named("Goblin")
                .FlavorText(
                  "When you're a goblin, you don't have to step forward to be a hero—everyone else just has to step back.")
                .Power(1)
                .Toughness(1)
                .Type("Token Creature - Goblin")
                .Colors(CardColor.Red));
          });
    }
  }
}