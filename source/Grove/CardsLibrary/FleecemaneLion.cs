namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class FleecemaneLion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fleecemane Lion")
        .ManaCost("{G}{W}")
        .Type("Creature — Cat")
        .Text("{3}{G}{W}: Monstrosity 1.{I}(If this creature isn't monstrous, put a +1/+1 counter on it and it becomes monstrous.){/I}{EOL}As long as Fleecemane Lion is monstrous, it has hexproof and indestructible.")
        .Power(3)
        .Toughness(3)
        .StaticAbility(p =>
        {
          p.Modifier(() => new AddSimpleAbility(Static.Hexproof));
          p.Modifier(() => new AddSimpleAbility(Static.Indestructible));
          p.Condition = cond => cond.OwningCardHas(Static.Monstrosity);
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{3}{G}{W}: Monstrosity 1.{I}(If this creature isn't monstrous, put a +1/+1 counter on it and it becomes monstrous.){/I}";
          p.Cost = new PayMana("{3}{G}{W}".Parse());
          p.Effect = () => new BecomeMonstrous(1);

          p.TimingRule(new WhenCardHas(c => !c.Has().Monstrosity));
          p.TimingRule(new Any(new PumpOwningCardTimingRule(1, 1), new OnEndOfOpponentsTurn()));
        });
    }
  }
}
