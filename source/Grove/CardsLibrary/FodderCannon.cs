namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class FodderCannon : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fodder Cannon")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text("{4},{T}, Sacrifice a creature: Fodder Cannon deals 4 damage to target creature.")
        .FlavorText(
          "Step 1: Find your cousin.{EOL}Step 2: Get your cousin in the cannon.{EOL}Step 3: Find another cousin.")
        .ActivatedAbility(p =>
          {
            p.Text = "{4},{T}, Sacrifice a creature: Fodder Cannon deals 4 damage to target creature.";

            p.Cost = new AggregateCost(
              new PayMana(4.Colorless()),
              new TapOwner(),
              new Sacrifice());

            p.Effect = () => new DealDamageToTargets(4);

            p.TargetSelector
              .AddCost(trg =>
                {
                  trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield();
                  trg.Message = "Select a creature to sacrifice.";
                })
              .AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new WhenStackIsNotEmpty()));
            p.TargetingRule(new CostSacrificeEffectDealDamage(4));
          });
    }
  }
}