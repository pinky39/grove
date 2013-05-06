namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Damage;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class Worship : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Worship")
        .ManaCost("{3}{W}")
        .Type("Enchantment")
        .Text(
          "If you control a creature, damage that would reduce your life total to less than 1 reduces it to 1 instead.")
        .FlavorText("Believe in the ideal, not the idol.")
        .Cast(p =>
          {
            p.TimingRule(new FirstMain());
            p.TimingRule(new ThereCanBeOnlyOne());
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddDamagePrevention(new PreventLifelossBelowOne());
            p.PlayerFilter = (player, effect) => player == effect.Source.Controller;
          });
    }
  }
}