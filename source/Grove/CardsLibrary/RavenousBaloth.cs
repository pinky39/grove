namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TargetingRules;

  public class RavenousBaloth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ravenous Baloth")
        .ManaCost("{2}{G}{G}")
        .Type("Creature - Beast")
        .Text("Sacrifice a Beast: You gain 4 life.")
        .FlavorText(
          "All we know about the Krosan Forest we have learned from those few who made it out alive.")
        .Power(4)
        .Toughness(4)
        .ActivatedAbility(p =>
          {
            p.Text = "Sacrifice a Beast: You gain 4 life.";
            p.Cost = new Sacrifice();
            p.Effect = () => new ChangeLife(amount: 4, whos: P(e => e.Controller));
            p.TargetSelector.AddCost(trg => trg.Is.Card(c => c.Is("beast"), ControlledBy.SpellOwner).On.Battlefield());
            p.TargetingRule(new CostSacrificeToGainLife());
          });
    }
  }
}