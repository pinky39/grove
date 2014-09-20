namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class SustainerOfTheRealm : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Trigger(new OnBlock(blocks: true));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(0, 2) {UntilEot = true});
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}