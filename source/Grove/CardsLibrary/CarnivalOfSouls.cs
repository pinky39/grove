namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class CarnivalOfSouls : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Carnival of Souls")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text("Whenever a creature enters the battlefield, you lose 1 life and add {B} to your mana pool.")
        .FlavorText(
          "Davvol, blast those elves.' ‘Davvol, transport those troops.' No one cares that today is my birthday.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a creature enters the battlefield, you lose 1 life and add {B} to your mana pool.";
            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              filter: (c, a, g) => c.Is().Creature));

            p.Effect = () => new CompoundEffect(
              new YouLooseLife(1),
              new AddManaToPool(Mana.Black));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}