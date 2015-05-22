namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class CrucibleOfFire : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Crucible of Fire")
        .ManaCost("{3}{R}")
        .Type("Enchantment")
        .Text("Dragon creatures you control get +3/+3.")
        .FlavorText("\"The dragon is a perfect marriage of power and the will to use it.\"—Sarkhan Vol")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.Effect = () => new CastPermanent().SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
          })
        .ContinuousEffect(p =>
          {
            p.Selector = (card, ctx) => card.Is("Dragon") && card.Controller == ctx.You;
            p.Modifier = () => new AddPowerAndToughness(3, 3);
          });
    }
  }
}