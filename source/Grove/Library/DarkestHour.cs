namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Modifiers;

  public class DarkestHour : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Darkest Hour")
        .Type("Enchantment")
        .ManaCost("{B}")
        .Text("All creatures are black")
        .FlavorText(
          "Yawgmoth spent eons wrapping Phyrexians in human skin. They are the sleeper agents, and they are everywhere.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new SetColors(CardColor.Black);
            p.CardFilter = (card, source) => card.Is().Creature;
          });
    }
  }
}