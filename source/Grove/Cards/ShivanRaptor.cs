namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;

  public class ShivanRaptor : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Shivan Raptor")
        .ManaCost("{2}{R}")
        .Type("Creature Lizard")
        .Text(
          "{First strike}, {haste}{EOL}Echo {2}{R} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.)")
        .Power(3)
        .Toughness(1)
        .Echo("{2}{R}")
        .Cast(p => p.TimingRule(new FirstMain()))
        .StaticAbilities(
          Static.Haste,
          Static.FirstStrike
        );
    }
  }
}