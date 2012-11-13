namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Modifiers;
  using Core.Dsl;

  public class GloriousAnthem : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Glorious Anthem")
        .ManaCost("{1}{W}{W}")
        .Type("Enchantment")
        .Text("Creatures you control get +1/+1.")
        .FlavorText("Once heard, the battle song of an angel becomes part of the listener forever.")
        .Category(EffectCategories.ToughnessIncrease)
        .Timing(Timings.FirstMain())
        .Abilities(
          Continuous(e =>
            {
              e.ModifierFactory = Modifier<AddPowerAndToughness>(
                m =>
                  {
                    m.Power = 1;
                    m.Toughness = 1;
                  });
              e.CardFilter = (card, source) => card.Controller == source.Controller && card.Is().Creature;
            }));
    }
  }
}