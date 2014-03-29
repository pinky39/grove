namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class BrineSeer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Brine Seer")
        .ManaCost("{3}{U}")
        .Type("Creature Human Wizard")
        .Text(
          "{2}{U},{T}: Reveal any number of blue cards in your hand. Counter target spell unless its controller pays {1} for each card revealed this way.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{2}{U},{T}: Reveal any number of blue cards in your hand. Counter target spell unless its controller pays {1} for each card revealed this way.";

            p.Cost = new AggregateCost(
              new PayMana("{2}{U}".Parse(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new CounterTargetSpellUnlessControllerPays1ForEachRevealedCard();
            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());
            
            
            p.TimingRule(new WhenTopSpellIsCounterable(tp => tp.Controller.Hand.Count(x => x.HasColor(CardColor.Blue))));
            p.TargetingRule(new EffectCounterspell());
          });
    }
  }
}