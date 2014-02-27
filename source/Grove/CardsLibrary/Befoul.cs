namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class Befoul : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Befoul")
        .ManaCost("{2}{B}{B}")
        .Type("Sorcery")
        .Text("Destroy target land or nonblack creature. It can't be regenerated.")
        .FlavorText("The land putrefied at its touch, turned into an oily bile in seconds.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents(canRegenerate: false)
              .SetTags(EffectTag.Destroy, EffectTag.CannotRegenerate);
            
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(card => card.Is().Land || (card.Is().Creature && !card.HasColor(CardColor.Black)))
              .On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}