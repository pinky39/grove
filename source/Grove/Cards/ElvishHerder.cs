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
        .Abilities(
          ActivatedAbility(
            "{G}: Target creature gains trample until end of turn.",
            Cost<PayMana>(cost => cost.Amount = "{G}".ParseMana()),
            Effect<Core.Effects.ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Trample,
                untilEndOfTurn: true))),
            Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()),
            targetingAi: TargetingAi.AddEvasion(),
            timing: Timings.NoRestrictions(),
            category: EffectCategories.ToughnessIncrease));        
    }
  }
}