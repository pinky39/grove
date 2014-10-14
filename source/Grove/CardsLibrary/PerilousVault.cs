namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class PerilousVault : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Perilous Vault")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text("{5},{T}, Exile Perilous Vault: Exile all nonland permanents.")
        .FlavorText("The spirit dragon Ugin arranged the hedrons of Zendikar to direct leylines of energy. To disrupt one is to unleash devastation and chaos.")
        .ActivatedAbility(p =>
        {
          p.Cost = new AggregateCost(
            new PayMana("{5}".Parse(), ManaUsage.Abilities),
            new Tap(),
            new Exile());

          p.Effect = () => new ExileAllCards(filter: (effect, card) => !card.Is().Land);

          p.TimingRule(new WhenOpponentControllsPermanents(selector: card => !card.Is().Land));
        });
    }
  }
}
