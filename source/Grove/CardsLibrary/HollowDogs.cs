namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class HollowDogs : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hollow Dogs")
        .ManaCost("{4}{B}")
        .Type("Creature Zombie Hound")
        .Text("Whenever Hollow Dogs attacks, it gets +2/+0 until end of turn.")
        .FlavorText(
          "A hollow dog is never empty. It is filled with thirst for the hunt.")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Hollow Dogs attacks, it gets +2/+0 until end of turn.";
            p.Trigger(new OnAttack());
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(2, 0) {UntilEot = true});
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}