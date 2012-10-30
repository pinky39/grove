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

  public class ElvishHerder : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Elvish Herder")
        .ManaCost("{G}")
        .Type("Creature Elf")
        .Text("{G}: Target creature gains trample until end of turn.")
        .FlavorText(
          "Before Urza and Mishra came to Argoth, the herders prevented their creatures from stampeding. During the war, they encouraged it.")
        .Power(1)
        .Toughness(1)
        .Timing(Timings.Creatures())
        .Abilities(
          C.ActivatedAbility(
            "{G}: Target creature gains trample until end of turn.",
            C.Cost<TapOwnerPayMana>(cost => cost.Amount = "{G}".ParseManaAmount()),
            C.Effect<ApplyModifiersToTargets>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = Static.Trample,
                untilEndOfTurn: true))),
            C.Validator(Validators.Creature()),
            selectorAi: TargetSelectorAi.AddEvasion(),
            timing: Timings.NoRestrictions(),
            category: EffectCategories.ToughnessIncrease));        
    }
  }
}