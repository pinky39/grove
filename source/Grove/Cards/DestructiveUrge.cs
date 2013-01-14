namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Messages;
  using Core.Targeting;
  using Core.Triggers;

  public class DestructiveUrge : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Destructive Urge")
        .ManaCost("{1}{R}{R}")
        .Type("Enchantment Aura")
        .Text(
          "{Enchant creature}{EOL}Whenever enchanted creature deals combat damage to a player, that player sacrifices a land.")
        .FlavorText("Red sky at night, dragon's delight.")
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<Core.Effects.Attach>();
            p.EffectTargets = L(Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.CombatEnchantment();
          })                                
        .Abilities(
          TriggeredAbility(
            "Whenever enchanted creature deals combat damage to a player, that player sacrifices a land.",
            Trigger<OnDamageDealt>(t =>
              {
                t.CombatOnly = true;
                t.UseAttachedToAsTriggerSource = true;
                t.ToPlayer();
              }),
            Effect<Core.Effects.PlayersSacrificeLands>(
              p =>
                {
                  p.Effect.OnlyPlayer = (Player) p.Parameters.Trigger<DamageHasBeenDealt>().Receiver;
                  p.Effect.Count = 1;
                }),
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}