namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;

  public class DarkestHour : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Darkest Hour")
        .Type("Enchantment")
        .ManaCost("{B}")
        .Text("All creatures are black")
        .FlavorText("'Yawgmoth spent eons wrapping Phyrexians in human skin. They are the sleeper agents, and they are everywhere.'{EOL}—Xantcha, to Urza")
        .Cast(p => p.Timing = Timings.FirstMain())     
        .Abilities(
          Continuous(e =>
            {
              e.CardFilter = (card, source) => card.Is().Creature;
              e.ModifierFactory = Modifier<SetColors>(
                m => { m.Colors = ManaColors.Black; });
            }));                
    }
  }
}