namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class ScentOfBrine : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scent of Brine")
        .ManaCost("{1}{U}")
        .Type("Instant")
        .Text(
          "Reveal any number of blue cards in your hand. Counter target spell unless its controller pays {1} for each card revealed this way.")
        .Cast(p =>
          {
            p.Effect = () => new CounterTargetSpellUnlessControllerPays1ForEachRevealedCard();
            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());

            p.TimingRule(new WhenTopSpellIsCounterable(tp => tp.Controller.Hand.Count(x => x.HasColor(CardColor.Blue))));
            p.TargetingRule(new EffectCounterspell());
          });
    }
  }
}