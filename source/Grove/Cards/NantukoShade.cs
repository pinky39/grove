namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Mana;
  using Core.Dsl;

  public class NantukoShade : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Nantuko Shade")
        .ManaCost("{B}{B}")
        .Type("Creature - Insect Shade")
        .Text("{B}: Nantuko Shade gets +1/+1 until end of turn.")
        .FlavorText("In life, the nantuko study nature by revering it. In death, they study nature by disemboweling it.")
        .Power(2)
        .Toughness(1)
        .Timing(Timings.Creatures())
        .Abilities(
          C.ActivatedAbility(
            "{B}: Nantuko Shade gets +1/+1 until end of turn.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.Amount = ManaUnit.Black.ToAmount()),
            C.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddPowerAndToughness>((m, _) =>
                {
                  m.Power = 1;
                  m.Toughness = 1;
                }, untilEndOfTurn: true))),
            category: EffectCategories.ToughnessIncrease,
            timing: Timings.IncreaseOwnersPowerAndThougness(1, 1)));
    }
  }
}