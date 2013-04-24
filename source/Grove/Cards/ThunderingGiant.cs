namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;

  public class ThunderingGiant : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Thundering Giant")
        .ManaCost("{3}{R}{R}")
        .Type("Creature Giant")
        .Text("{Haste}")
        .FlavorText("The giant was felt a few seconds before he was seen.")
        .Power(4)
        .Toughness(3)
        .Cast(p => p.TimingRule(new FirstMain()))
        .StaticAbilities(Static.Haste);
    }
  }
}