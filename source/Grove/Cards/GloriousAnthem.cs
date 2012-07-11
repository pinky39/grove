namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;

  public class GloriousAnthem : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Glorious Anthem")
        .ManaCost("{1}{W}{W}")
        .Type("Enchantment")
        .Text("Creatures you control get +1/+1.")
        .FlavorText("Once heard, the battle song of an angel becomes part of the listener forever.")
        .Category(EffectCategories.ToughnessIncrease)
        .Timing(Timings.FirstMain())
        .Abilities(
          C.Continuous((e, c) =>
            {
              e.ModifierFactory = c.Modifier<AddPowerAndToughness>(
                (m, _) =>
                  {
                    m.Power = 1;
                    m.Toughness = 1;
                  });
              e.CardFilter = (card, source) => card.Controller == source.Controller;
            }));
    }
  }
}