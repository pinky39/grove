namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class CaveTiger : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Cave Tiger")
        .ManaCost("{2}{G}")
        .Type("Creature Cat")
        .Text("Whenever Cave Tiger becomes blocked by a creature, Cave Tiger gets +1/+1 until end of turn.")
        .FlavorText(
          "The druids found a haven in the cool limestone tunnels beneath Argoth. The invaders found only tigers.")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Cave Tiger becomes blocked by a creature, Cave Tiger gets +1/+1 until end of turn.";
            p.Trigger(new WhenThisBecomesBlocked(triggerForEveryBlocker: true));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, 1) {UntilEot = true});
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}