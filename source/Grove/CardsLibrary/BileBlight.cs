namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class BileBlight : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bile Blight")
        .ManaCost("{B}{B}")
        .Type("Instant")
        .Text("Target creature and all other creatures with the same name as that creature get -3/-3 until end of turn.")
        .FlavorText("Not an arrow loosed, javelin thrown, nor sword raised. None were needed.")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(           
            new ApplyModifiersToPermanents(
              selector: (c, ctx) => ctx.Target.Card().Name == c.Name,
              modifier: () => new AddPowerAndToughness(-3, -3) {UntilEot = true}))
          {
            ToughnessReduction = 3
          }.SetTags(EffectTag.ReduceToughness);

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          
          p.TargetingRule(new EffectReduceToughness(3));
          p.TimingRule(new TargetRemovalTimingRule(EffectTag.ReduceToughness));
        });
    }
  }
}
