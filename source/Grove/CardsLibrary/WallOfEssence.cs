namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Events;
  using Triggers;

  public class WallOfEssence : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wall of Essence")
        .ManaCost("{1}{W}")
        .Type("Creature - Wall")
        .Text(
          "{Defender}{I}(This creature can't attack.){/I}{EOL}Whenever Wall of Essence is dealt combat damage, you gain that much life.")
        .FlavorText(
          "\"If you cannot turn your enemy's strength to weakness, then make that strength your own.\"{EOL}—Gresha, warrior sage")
        .Power(0)
        .Toughness(4)
        .SimpleAbilities(Static.Defender)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Wall of Essence is dealt combat damage, you gain that much life.";

            p.Trigger(new OnDamageDealt(dmg =>
              dmg.IsCombat &&
                dmg.IsDealtToOwningCard));

            p.Effect =
              () => new ChangeLife(amount: P(e => e.TriggerMessage<DamageDealtEvent>().Damage.Amount), forYou: true);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}