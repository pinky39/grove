namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class EndlessObedience : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Endless Obedience")
        .ManaCost("{4}{B}{B}")
        .Type("Sorcery")
        .Text("{Convoke} {I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}Put target creature card from a graveyard onto the battlefield under your control.")
        .FlavorText("The death of a scout can be as informative as a safe return.")
        .Convoke()
        .Cast(p =>
        {
          p.Text = "Put target creature card from a graveyard onto the battlefield under your control.";

          p.Effect = () => new PutTargetsToBattlefield();

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Graveyard());

          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
        });
    }
  }
}
