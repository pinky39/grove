namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;

  public class AbsoluteGrace : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Absolute Grace")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text("All creatures have protection from black.")
        .FlavorText(
          "In pursuit of Urza, the Phyrexians sent countless foul legions into Serra's realm. Though beaten back, they left it tainted with uncleansable evil.")
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
                m => { m.Colors = ManaColors.Black; });
            }));
    }
  }
}