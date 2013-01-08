namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;

  public class NantukoShade : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Nantuko Shade")
        .ManaCost("{B}{B}")
        .Type("Creature - Insect Shade")
        .Text("{B}: Nantuko Shade gets +1/+1 until end of turn.")
        .FlavorText("In life, the nantuko study nature by revering it. In death, they study nature by disemboweling it.")
        .Power(2)
        .Toughness(1)        
        .Abilities(
          ActivatedAbility(
            "{B}: Nantuko Shade gets +1/+1 until end of turn.",
            Cost<PayMana>(cost => cost.Amount = ManaUnit.Black.ToAmount()),
            Effect<Core.Cards.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
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