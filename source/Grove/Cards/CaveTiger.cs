namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;

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
            p.Trigger(new OnBlock(becomesBlocked: true, triggerForEveryCreature: true));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, 1) {UntilEot = true});
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}