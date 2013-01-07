namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;

  public class AbsoluteLaw : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Absolute Law")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text("All creatures have protection from red.")
        .FlavorText(
          "The strength of law is unwavering. It is an iron bar in a world of water.")
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Category = EffectCategories.Protector;
          })
        .Abilities(
          Continuous(e =>
            {
              e.CardFilter = (card, source) => card.Is().Creature;
              e.ModifierFactory = Modifier<AddProtectionFromColors>(
                m => { m.Colors = ManaColors.Red; });
            }));
    }
  }
}