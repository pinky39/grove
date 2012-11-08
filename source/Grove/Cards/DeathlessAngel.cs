namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Mana;
  using Core.Dsl;
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
            Effect<ApplyModifiersToTargets>(p => p.Effect.Modifiers(
              Modifier<AddStaticAbility>(m => { m.StaticAbility = Static.Indestructible; },
                untilEndOfTurn: true))),
            Validator(validator: Validators.Creature()),
            selectorAi: TargetSelectorAi.ShieldIndestructible(),
            timing: Timings.NoRestrictions(),
            category: EffectCategories.Protector));
    }
  }
}