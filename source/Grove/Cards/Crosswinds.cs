namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Modifiers;

  public class Crosswinds : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Crosswinds")
        .ManaCost("{1}{G}")
        .Type("Enchantment")
        .Text("Creatures with flying get -2/-0.")
        .FlavorText(
          "Harbin's ornithopter had been trapped for two days within the currents of the storm. When the skies cleared, all he could see was a horizon of trees.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddPowerAndToughness(-2, 0);
            p.CardFilter = (card, source) => card.Is().Creature && card.Has().Flying;
          });
    }
  }
}