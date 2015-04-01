namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Modifiers;
  using Triggers;

  public class HeirOfTheWilds : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Heir of the Wilds")
        .ManaCost("{1}{G}")
        .Type("Creature — Human Warrior")
        .Text("{Deathtouch}{EOL}{I}Ferocious{/I} — Whenever Heir of the Wilds attacks, if you control a creature with power 4 or greater, Heir of the Wilds gets +1/+1 until end of turn.")
        .FlavorText("In the high caves of the Qal Sisma mountains, young hunters quest to hear the echoes of their fierce ancestors.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Deathtouch)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Heir of the Wilds attacks, if you control a creature with power 4 or greater, Heir of the Wilds gets +1/+1 until end of turn.";

          p.Trigger(new WhenThisAttacks()
          {
            Condition = ctx => ctx.You.Battlefield.Creatures.Any(x => x.Power >= 4)
          });

          p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, 1) { UntilEot = true });
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
