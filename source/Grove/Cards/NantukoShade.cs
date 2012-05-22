namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Effects;
  using Core.Modifiers;

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
        .Abilities(
          C.ActivatedAbility(
            "{B}: Nantuko Shade gets +1/+1 until end of turn.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.Amount = Mana.Black),
            C.Effect<ApplyModifiersToSelf>((e, c) => e.Modifiers(
              c.Modifier<AddPowerAndToughness>((m, _) => {
                m.Power = 1;
                m.Toughness = 1;
              }, untilEndOfTurn: true))),
            timing: Any(
              Timings.ResponseToSpell(EffectCategories.LifepointReduction),
              Timings.Combat)));
    }
  }
}