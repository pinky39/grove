namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.CostRules;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class MishrasHelix : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mishra's Helix")
        .ManaCost("{5}")
        .Type("Artifact")
        .Text("{X},{T}: Tap X target lands.")
        .FlavorText(
          "The helix was the finest example of Mishra's campaign strategy: if he couldn't have Argoth, no one could.")
        .ActivatedAbility(p =>
          {
            p.Text = "{X},{T}: Tap X target lands.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.Zero, hasX: true),
              new Tap());

            p.Effect = () => new TapTargets();

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Land).On.Battlefield(),
              trg => {
                trg.MinCount = Value.PlusX;
                trg.MaxCount = Value.PlusX;                
              });

            p.TimingRule(new OnOpponentsTurn(Step.Upkeep));
            p.TimingRule(new WhenOpponentControllsPermanents(x => x.Is().Land));
            p.CostRule(new XIsOpponentsLandCount());
            p.TargetingRule(new EffectTapLand());
          });
    }
  }
}