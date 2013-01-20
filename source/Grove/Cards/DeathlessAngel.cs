namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;
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
        .Abilities(
          Static.Flying,
          ActivatedAbility(
            "{W}{W}: Target creature is indestructible this turn.",
            Cost<PayMana>(cost => cost.Amount = "{W}{W}".ParseMana()),
            Effect<Core.Effects.ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddStaticAbility>(m => { m.StaticAbility = Static.Indestructible; },
                untilEndOfTurn: true))),
            Target(
              Validators.Card(x => x.Is().Creature),
              Zones.Battlefield()),
            targetingAi: TargetingAi.ShieldIndestructible(),
            timing: Timings.NoRestrictions(),
            category: EffectCategories.Protector));
    }
  }
}