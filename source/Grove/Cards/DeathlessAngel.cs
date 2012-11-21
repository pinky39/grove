namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Costs;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class DeathlessAngel : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Deathless Angel")
        .ManaCost("{4}{W}{W}")
        .Type("Creature Angel")
        .Text("{Flying}{EOL}{W}{W}: Target creature is indestructible this turn.")
        .FlavorText(
          "'I should have died that day, but I suffered not a scratch. I awoke in a lake of blood, none of it apparently my own.'{EOL}—The War Diaries")
        .Power(5)
        .Toughness(7)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Flying,
          ActivatedAbility(
            "{W}{W}: Target creature is indestructible this turn.",
            Cost<TapOwnerPayMana>(cost => cost.Amount = "{W}{W}".ParseManaAmount()),
            Effect<Core.Cards.Effects.ApplyModifiersToTargets>(p => p.Effect.Modifiers(
              Modifier<AddStaticAbility>(m => { m.StaticAbility = Static.Indestructible; },
                untilEndOfTurn: true))),
            TargetValidator(target: TargetIs.Creature()),
            targetSelectorAi: TargetSelectorAi.ShieldIndestructible(),
            timing: Timings.NoRestrictions(),
            category: EffectCategories.Protector));
    }
  }
}