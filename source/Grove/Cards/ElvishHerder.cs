namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class ElvishHerder : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
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
          ActivatedAbility(
            "{G}: Target creature gains trample until end of turn.",
            Cost<TapOwnerPayMana>(cost => cost.Amount = "{G}".ParseManaAmount()),
            Effect<ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Trample,
                untilEndOfTurn: true))),
            Validator(Validators.Creature()),
            selectorAi: TargetSelectorAi.AddEvasion(),
            timing: Timings.NoRestrictions(),
            category: EffectCategories.ToughnessIncrease));        
    }
  }
}