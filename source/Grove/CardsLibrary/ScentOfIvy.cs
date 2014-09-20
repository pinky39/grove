namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class ScentOfIvy : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scent of Ivy")
        .ManaCost("{G}")
        .Type("Instant")
        .Text(
          "Reveal any number of green cards in your hand. Target creature gets +X/+X until end of turn, where X is the number of cards revealed this way.")
        .Cast(p =>
          {
            p.Effect = () => new CreatureGetsPwtForEachRevealedCard(1, 1, c => c.HasColor(CardColor.Green));
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.HasColor(CardColor.Green)));

            p.TargetingRule(new EffectPumpInstant(
              power: tp => tp.Controller.Hand.Count(c => c.HasColor(CardColor.Green)),
              toughness: tp => tp.Controller.Hand.Count(c => c.HasColor(CardColor.Green))));
          });
    }
  }
}