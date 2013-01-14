namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;

  public class LoomingShade :CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Looming Shade")
        .ManaCost("{2}{B}")
        .Type("Creature - Shade")
        .Text("{B}: Looming Shade gets +1/+1 until end of turn.")
        .FlavorText("The shade tracks victims by reverberations of the pipes, as a spider senses prey tangled in its trembling web.")
        .Power(1)
        .Toughness(1)        
        .Abilities(
          ActivatedAbility(
            "{B}: Looming Shade gets +1/+1 until end of turn.",
            Cost<PayMana>(cost => cost.Amount = ManaAmount.Black),
            Effect<Core.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 1;
                  m.Toughness = 1;
                }, untilEndOfTurn: true))),
            category: EffectCategories.ToughnessIncrease,
            timing: Timings.IncreaseOwnersPowerAndThougness(1, 1)));
    }
  }
}