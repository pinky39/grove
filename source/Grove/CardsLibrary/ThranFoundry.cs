namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class ThranFoundry : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thran Foundry")
        .ManaCost("{1}")
        .Type("Artifact")
        .Text("{1},{T}, Exile Thran Foundry: Target player shuffles his or her graveyard into his or her library.")
        .FlavorText("What we do not use up, we use again.")
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1},{T}, Exile Thran Foundry: Target player shuffles his or her graveyard into his or her library.";
            p.Cost = new AggregateCost(
              new PayMana(1.Colorless()),
              new Tap(),
              new Exile());

            p.Effect = () => new ShuffleTargetGraveyardIntoLibrary(c => true);

            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TargetingRule(new EffectAnyPlayer());
            p.TimingRule(new OnEndOfOpponentsTurn());
          });
    }
  }
}