namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class TormodsCrypt : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Tormod's Crypt")
        .ManaCost("{0}")
        .Type("Artifact")
        .Text("{T}, Sacrifice Tormod's Crypt: Exile all cards from target player's graveyard.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
        {
          p.Text = "{T}, Sacrifice Tormod's Crypt: Exile all cards from target player's graveyard.";
          p.Cost = new AggregateCost(
            new Tap(),
            new Sacrifice());

          p.Effect = () => new ExileAllCards(
            from: Zone.Graveyard);

          p.TargetSelector.AddEffect(trg => trg.Is.Player());
          p.TargetingRule(new EffectOpponent());
        });
    }
  }
}
