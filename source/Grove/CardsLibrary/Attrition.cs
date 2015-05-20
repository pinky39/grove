namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class Attrition : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Attrition")
        .ManaCost("{1}{B}{B}")
        .Type("Enchantment")
        .Text("{B}, Sacrifice a creature: Destroy target nonblack creature.")
        .FlavorText("I will trade life for life with the insurgents. Our resources, unlike theirs, are limitless.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{B}, Sacrifice a creature: Destroy target nonblack creature.";
            p.Cost = new AggregateCost(
              new PayMana(Mana.Black),
              new Sacrifice());

            p.Effect = () => new DestroyTargetPermanents();

            p.TargetSelector.AddCost(
              trg => trg.Is.Card(c => c.Is().Creature, ControlledBy.SpellOwner).On.Battlefield(),
              trg => trg.Message = "Select a creature to sacrifice.");

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Creature && !c.HasColor(CardColor.Black)).On.Battlefield(),
              trg => trg.Message = "Select a creature to destroy.");

            p.TargetingRule(new CostSacrificeEffectDestroy());

            p.TimingRule(new TargetRemovalTimingRule()
              .RemovalTags(EffectTag.Destroy, EffectTag.CreaturesOnly));
          });
    }
  }
}