namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;

  public class DarkRitual : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dark Ritual")
        .ManaCost("{B}")
        .Type("Instant")
        .Text("Add {B}{B}{B} to your mana pool.")
        .FlavorText(
          "From void evolved Phyrexia. Great Yawgmoth, Father of Machines, saw its perfection. Thus The Grand Evolution began.")
        .OverrideScore(new ScoreOverride {Hand = 80})
        /* ritual score must be lowered a bit so ai casts it more eagerly */
        .Cast(p =>
          {
            p.TimingRule(new WhenYouNeedAdditionalMana(2));
            p.Effect = () => new AddManaToPool("{B}{B}{B}".Parse());
          });
    }
  }
}