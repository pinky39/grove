namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Messages;

  public class DestructiveUrge : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
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
              validator: c=> c.Is().Land,
              text: "Select a land to sacrifice.",
              playerFilter: (e, player) => e.TriggerMessage<DamageHasBeenDealt>().Receiver == player);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}