namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class Befoul : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Befoul")
        .ManaCost("{2}{B}{B}")
        .Type("Sorcery")
        .Text("Destroy target land or nonblack creature. It can't be regenerated.")
        .FlavorText("The land putrefied at its touch, turned into an oily bile in seconds.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents(canRegenerate: false) {Category = EffectCategories.Destruction};
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(card => card.Is().Land || (card.Is().Creature && !card.HasColors(ManaColors.Black)))
              .On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new Destroy());
          });
    }
  }
}