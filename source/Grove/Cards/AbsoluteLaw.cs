namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class AbsoluteLaw : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Absolute Law")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text("All creatures have protection from red.")
        .FlavorText(
          "The strength of law is unwavering. It is an iron bar in a world of water.")
       .Cast(p =>
          {
            p.TimingRule(new FirstMain());
            p.Effect = () => new PutIntoPlay {Category = EffectCategories.Protector};
          })
        .ContinuousEffect(p =>
          {
            p.CardFilter = (card, source) => card.Is().Creature;
            p.ModifierFactory = () => new AddProtectionFromColors(ManaColors.Red);
          });
    }
  }
}