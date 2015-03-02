namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class CranialArchive : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Cranial Archive")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text("{2},Exile Cranial Archive: Target player shuffles his or her graveyard into his or her library. Draw a card.")
        .FlavorText("The greatest idea the zombie ever had in its head wasn't even its own.")
        .ActivatedAbility(p =>
        {
          p.Text = "{2},Exile Cranial Archive: Target player shuffles his or her graveyard into his or her library. Draw a card.";
          p.Cost = new AggregateCost(
            new PayMana(2.Colorless()),
            new Costs.Exile());

          p.Effect = () => new CompoundEffect(
            new ShuffleTargetGraveyardIntoLibrary(c => true),
            new DrawCards(1));

          p.TargetSelector.AddEffect(trg => trg.Is.Player());
          p.TargetingRule(new EffectAnyPlayer());
          p.TimingRule(new OnEndOfOpponentsTurn());
        });
    }
  }
}
