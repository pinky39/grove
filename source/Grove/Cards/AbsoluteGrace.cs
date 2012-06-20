namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;
  using Core.Modifiers;

  public class AbsoluteGrace : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Absolute Grace")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text("All creatures have protection from black.")
        .FlavorText(
          "In pursuit of Urza, the Phyrexians sent countless foul legions into Serra's realm. Though beaten back, they left it tainted with uncleansable evil.")
        .Abilities(
          C.Continuous((e, c) =>
            {
              e.ModifierFactory = c.Modifier<AddProtectionFromColors>(
                (m, _) => { m.Colors = ManaColors.Black; });
            }));
    }
  }
}