namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Preventions;
  using Core.Dsl;
  using Core.Targeting;

  public class HealingSalve : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Healing Salve")
        .ManaCost("{W}")
        .Type("Instant")
        .Text(
          "Choose one — Target player gains 3 life; or prevent the next 3 damage that would be dealt to target creature or player this turn.")
        .FlavorText(
          "'Xantcha is recovering. The medicine is slow, but my magic would have killed her'{EOL}—Serra, to Urza")
        .Cast(p =>
          {
            p.Description = "Target player gains 3 life";
            p.Effect = Effect<TargetPlayerGainsLife>(e => e.Amount = 3);
            p.EffectTargets = L(Target(Validators.Player(), Zones.None()));
            p.TargetingAi = TargetingAi.Controller();
          })
        .Cast(p =>
          {
            p.Description = "Prevent the next 3 damage that would be dealt to target creature or player this turn.";
            p.Effect = Effect<ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddDamagePrevention>(m => m.Prevention =
                Prevention<PreventDamageToTarget>(pr => { pr.Amount = 3; }), untilEndOfTurn: true)));
            p.EffectTargets = L(Target(Validators.CreatureOrPlayer(), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.PreventNextDamageToCreatureOrPlayer(3);
          });
    }
  }
}