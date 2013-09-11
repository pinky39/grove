namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;

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