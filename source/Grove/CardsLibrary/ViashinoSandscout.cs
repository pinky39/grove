namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Triggers;

  public class ViashinoSandscout : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Viashino Sandscout")
        .Type("Creature Viashino Scout")
        .ManaCost("{1}{R}")
        .Text("{Haste}{EOL}At the beginning of the end step, return Viashino Sandscout to its owner's hand.")
        .Power(2)
        .Toughness(1)
        .SimpleAbilities(Static.Haste)
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of the end step, return Viashino Sandscout to its owner's hand.";
            p.Trigger(new OnStepStart(
              step: Step.EndOfTurn,
              activeTurn: true,
              passiveTurn: true
              ));

            p.Effect = () => new ReturnToHand(returnOwningCard: true);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}