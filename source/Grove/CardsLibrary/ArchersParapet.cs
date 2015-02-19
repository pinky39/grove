namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class ArchersParapet : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Archers' Parapet")
        .ManaCost("{1}{G}")
        .Type("Creature — Wall")
        .Text("{Defender}{EOL}{1}{B},{T}: Each opponent loses 1 life.")
        .FlavorText("Every shaft is graven with a name from a kin tree, calling upon the spirits of the ancestors to make it fly true.")
        .Power(0)
        .Toughness(5)
        .SimpleAbilities(Static.Defender)
        .ActivatedAbility(p =>
        {
          p.Text = "{1}{B},{T}: Each opponent loses 1 life.";

          p.Cost = new AggregateCost(
            new PayMana("{1}{B}".Parse()),
            new Tap());

          p.Effect = () => new ChangeLife(-1, opponents: true);

          p.TimingRule(new OnEndOfOpponentsTurn());
        });
    }
  }
}
