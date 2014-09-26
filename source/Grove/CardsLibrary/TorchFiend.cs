namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class TorchFiend : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Torch Fiend")
        .ManaCost("{1}{R}")
        .Type("Creature — Devil")
        .Text("{R}, Sacrifice Torch Fiend: Destroy target artifact.")
        .FlavorText("Devils redecorate every room with fire.")
        .Power(2)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{R}, Sacrifice Torch Fiend: Destroy target artifact.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.Red, ManaUsage.Abilities),
              new Sacrifice());

            p.Effect = () => new DestroyTargetPermanents();

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Artifact).On.Battlefield());

            p.TimingRule(new WhenPermanentCountIs(c => c.Is().Artifact));
            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}