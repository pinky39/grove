namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;
  using Core.Modifiers;

  public class AbsoluteLaw : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Absolute Law")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text("All creatures have protection from red.")
        .FlavorText(
          "The strength of law is unwavering. It is an iron bar in a world of water.")
        .Abilities(
          C.Continuous((e, c) =>
            {
              e.ModifierFactory = c.Modifier<AddProtectionFromColors>(
                (m, _) => { m.Colors = ManaColors.Red; });
            }));
    }
  }
}