namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class Metalworker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Metalworker")
        .ManaCost("{3}")
        .Type("Artifact Creature Construct")
        .Text(
          "{T}: Reveal any number of artifact cards in your hand. Add {2} to your mana pool for each card revealed this way.")
        .FlavorText("At this rate I fully expect to be replaced by a clockwork golem by year's end.")
        .Power(1)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}: Reveal any number of artifact cards in your hand. Add {2} to your mana pool for each card revealed this way.";

            p.Cost = new Tap();
            p.Effect = () => new AddManaForEachRevealedCard(c => c.Is().Artifact, 2);
            p.TimingRule(new WhenYouNeedAdditionalMana());
            p.UsesStack = false;
          });
    }
  }
}