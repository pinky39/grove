namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class Raze : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Raze")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text("As an additional cost to cast Raze, sacrifice a land.{EOL}Destroy target land.")
        .FlavorText("The viashino believe that the oldest mountains hate everyone equally.")
        .OverrideScore(p => p.Hand = 50)
        .Cast(p =>
          {
            p.Cost = new AggregateCost(
              new PayMana(Mana.Red),
              new Sacrifice());

            p.Effect = () => new DestroyTargetPermanents();

            p.TargetSelector.AddCost(trg =>
              {
                trg.Is.Card(c => c.Is().Land, ControlledBy.SpellOwner).On.Battlefield();
                trg.Message = "Select a land to sacrifice.";
              });

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card(c => c.Is().Land).On.Battlefield();
                trg.Message = "Select a land to destroy.";
              });

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new CostSacrificeEffectDestroy());
          });
    }
  }
}