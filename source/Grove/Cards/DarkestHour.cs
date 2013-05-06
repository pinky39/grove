namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Characteristics;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class DarkestHour : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Darkest Hour")
        .Type("Enchantment")
        .ManaCost("{B}")
        .Text("All creatures are black")
        .FlavorText(
          "Yawgmoth spent eons wrapping Phyrexians in human skin. They are the sleeper agents, and they are everywhere.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new SetColors(CardColor.Black);
            p.CardFilter = (card, source) => card.Is().Creature;
          });
    }
  }
}