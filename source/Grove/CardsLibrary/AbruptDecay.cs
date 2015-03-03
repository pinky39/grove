namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class AbruptDecay : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Abrupt Decay")
        .ManaCost("{B}{G}")
        .Type("Instant")
        .Text("Abrupt Decay can't be countered by spells or abilities.{EOL}Destroy target nonland permanent with converted mana cost 3 or less.")
        .FlavorText("The Izzet quickly suspended their policy of lifetime guarantees.")
        .Cast(p =>
        {
          p.Effect = () => new DestroyTargetPermanents(){ CanBeCountered = false };
          p.TargetSelector.AddEffect(trg => trg
            .Is.Card(c => !c.Is().Land && c.ConvertedCost <= 3)
            .On.Battlefield());

          p.TargetingRule(new EffectDestroy());
          p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Destroy));
        });
    }
  }
}
