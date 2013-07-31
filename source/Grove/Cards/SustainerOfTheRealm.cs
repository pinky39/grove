namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;

  public class SustainerOfTheRealm : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Sustainer of the Realm")
        .ManaCost("{2}{W}{W}")
        .Type("Creature Angel")
        .Text("{Flying}{EOL}Whenever Sustainer of the Realm blocks, it gets +0/+2 until end of turn.")
        .FlavorText("The harder you push, the stronger we become.")
        .Power(2)
        .Toughness(3)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Sustainer of the Realm blocks, it gets +0/+2 until end of turn.";
            p.Trigger(new OnBlock(becomesBlocked: true));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(0, 2) {UntilEot = true});
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}