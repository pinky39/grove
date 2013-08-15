namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class Levitation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Levitation")
        .ManaCost("{2}{U}{U}")
        .Type("Enchantment")
        .Text("Creatures you control have flying.")
        .FlavorText("Barrin's pride in his apprentice was diminished somewhat when he had to get the others back down.")
        .Cast(p =>
          {
            p.TimingRule(new FirstMain());
            p.TimingRule(new ThereCanBeOnlyOne());
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddStaticAbility(Static.Flying);
            p.CardFilter = (card, effect) => card.Controller == effect.Source.Controller && card.Is().Creature;
          });
    }
  }
}