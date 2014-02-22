namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;
  using Grove.Gameplay.Triggers;

  public class ViashinoWeaponsmith : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Viashino Weaponsmith")
        .ManaCost("{3}{R}")
        .Type("Creature Viashino")
        .Text("Whenever Viashino Weaponsmith becomes blocked by a creature, Viashino Weaponsmith gets +2/+2 until end of turn.")
        .FlavorText("Within the rig settlement, those who have mastered the making of weapons earn highest honor.")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Viashino Weaponsmith becomes blocked by a creature, Viashino Weaponsmith gets +2/+2 until end of turn.";
            p.Trigger(new OnBlock(becomesBlocked: true, triggerForEveryCreature: true));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(2, 2) { UntilEot = true });
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}