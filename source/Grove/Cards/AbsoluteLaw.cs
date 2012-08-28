namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Mana;
  using Core.Dsl;

  public class AbsoluteLaw : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Absolute Law")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text("All creatures have protection from red.")
        .Timing(Timings.FirstMain())
        .FlavorText(
          "The strength of law is unwavering. It is an iron bar in a world of water.")
        .Category(EffectCategories.Protector)
        .Abilities(
          C.Continuous((e, c) =>
            {
              e.CardFilter = (card, source) => card.Is().Creature;
              e.ModifierFactory = c.Modifier<AddProtectionFromColors>(
                (m, _) => { m.Colors = ManaColors.Red; });
            }));
    }
  }
}