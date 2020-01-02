namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class KiorasFollower : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Kiora's Follower")
          .ManaCost("{G}{U}")
          .Type("Creature — Merfolk")
          .Text("{T}: Untap another target permanent.")
          .FlavorText("\"She may call herself Kiora but I believe she is Thassa, the embodiment of the sea and empress of the depths.\"")
          .Power(2)
          .Toughness(2)
          .ActivatedAbility(p =>
          {
            p.Text = "{T}: Untap another target permanent.";
            p.Cost = new Tap();

            p.Effect = () => new UntapTargetPermanents();

            p.TargetSelector.AddEffect(trg => trg.Is.Card(canTargetSelf: false).On.Battlefield());
            
            p.TimingRule(new Any(
                new OnFirstMain(),
                new AfterOpponentDeclaresAttackers()));

            p.TargetingRule(new EffectUntapPermanent());
          });
    }
  }
}
