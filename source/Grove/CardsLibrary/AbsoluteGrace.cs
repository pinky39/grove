namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class AbsoluteGrace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.TimingRule(new OnFirstMain());
            p.Effect = () => new CastPermanent().SetTags(EffectTag.Protection);
          })
        .ContinuousEffect(p =>
          {
            p.Selector = (card, ctx) => card.Is().Creature;
            p.Modifier = () => new AddProtectionFromColors(CardColor.Black);
          });
    }
  }
}