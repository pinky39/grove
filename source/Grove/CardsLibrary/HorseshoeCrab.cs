namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;

  public class HorseshoeCrab : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Horseshoe Crab")
        .ManaCost("{2}{U}")
        .Type("Creature - Crab")
        .Text("{U}: Untap Horseshoe Crab.")
        .FlavorText(
          "In the final days before the disaster, all the crabs on Tolaria migrated from inlets, streams, and ponds back to the sea. No one took note.")
        .Power(1)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text = "{U}: Untap Horseshoe Crab.";
            p.Cost = new PayMana(Mana.Blue);
            p.Effect = () => new UntapOwner();
            
            p.TimingRule(new OnSecondMain());
            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => c.IsTapped));
          });
    }
  }
}