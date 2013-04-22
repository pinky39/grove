namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

  public class AbsoluteGrace : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.TimingRule(new FirstMain());
            p.Effect = () => new PutIntoPlay {Category = EffectCategories.Protector};
          })
        .ContinuousEffect(p =>
          {
            p.CardFilter = (card, source) => card.Is().Creature;
            p.Modifier = () => new AddProtectionFromColors(CardColor.Black);
          });
    }
  }
}