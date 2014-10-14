namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class YavimayaCoast : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Yavimaya Coast")
        .Type("Land")
        .Text("{T}: Add {1} to your mana pool.{EOL}{T}: Add {G} or {U} to your mana pool. Battlefield Forge deals 1 damage to you.")
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {1} to your mana pool.";
          p.ManaAmount(1.Colorless());
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{T}: Add {G} or {U} to your mana pool. Battlefield Forge deals 1 damage to you.";
          p.Cost = new Tap();
          p.Effect = () => new CompoundEffect(
            new AddManaToPool(Mana.Colored(isGreen: true, isBlue: true)),
            new DealDamageToPlayer(1, P(e => e.Controller)));
          p.TimingRule(new WhenYouNeedAdditionalMana(1));
          p.UsesStack = false;
        });
    }
  }
}
