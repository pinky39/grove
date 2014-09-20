namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Events;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class DestructiveUrge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Destructive Urge")
        .ManaCost("{1}{R}{R}")
        .Type("Enchantment Aura")
        .Text(
          "Whenever enchanted creature deals combat damage to a player, that player sacrifices a land.")
        .FlavorText("Red sky at night, dragon's delight.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever enchanted creature deals combat damage to a player, that player sacrifices a land.";

            p.Trigger(new OnDamageDealt(
              combatOnly: true,
              useAttachedToAsTriggerSource: true,
              playerFilter: delegate { return true; }));

            p.Effect = () => new PlayersSacrificePermanents(
              count: 1,
              validator: c => c.Is().Land,
              text: "Select a land to sacrifice.",
              playerFilter: (e, player) => e.TriggerMessage<DamageDealtEvent>().Receiver == player);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}