namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class DevoutHarpist : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Devout Harpist")
        .ManaCost("{W}")
        .Type("Creature Human")
        .Text("{T}: Destroy target Aura attached to a creature.")
        .FlavorText(
          "The notes themselves are irrelevant. The music's effect, however, is worth ten armies.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Destroy target Aura attached to a creature.";
            p.Cost = new Tap();
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Aura).On.Battlefield());

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule(EffectTag.Destroy));
          }
        );
    }
  }
}