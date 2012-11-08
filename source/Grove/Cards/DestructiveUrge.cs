namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Messages;
  using Core.Targeting;

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
        .Timing(Timings.FirstMain())
        .Effect<Attach>()
        .Targets(
          selectorAi: TargetSelectorAi.CombatEnchantment(),
          effectValidator: Validator(Validators.EnchantedCreature()))
        .Abilities(
          TriggeredAbility(
            "Whenever enchanted creature deals combat damage to a player, that player sacrifices a land.",
            Trigger<DealDamageToCreatureOrPlayer>(t =>
              {
                t.CombatOnly = true;
                t.UseAttachedToAsTriggerSource = true;
                t.ToPlayer();
              }),
            Effect<PlayerSacrificeLands>(
              p =>
                {
                  p.Effect.Player = (Player) p.Parameters.Trigger<DamageHasBeenDealt>().Receiver;
                  p.Effect.Count = 1;
                }),
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}