namespace Grove.CardsLibrary
{
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using System.Collections.Generic;

  public class JaceTheLivingGuildpact : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jace, the Living Guildpact")
        .ManaCost("{2}{U}{U}")
        .Type("Planeswalker Jace")
        .Text("{+1}: Look at the top two cards of your library. Put one of them into your graveyard.{EOL}" +
        "{-3}: Return another target nonland permanent to its owner's hand.{EOL}" +
        "{-8}: Each player shuffles their hand and graveyard into their library. You draw seven cards.")
        .Loyality(5)
        .ActivatedAbility(p =>
        {
          p.Text = "{+1}: Look at the top two cards of your library. Put one of them into your graveyard.";
          p.Cost = new AddCountersCost(CounterType.Loyality, 1);
          p.Effect = () => new PutSelectedCardsIntoGraveyardOthersOnTop(2, 1);
          p.TimingRule(new OnFirstMain());
          p.ActivateAsSorcery = true;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{-3}: Return another target nonland permanent to its owner's hand.";
          p.Cost = new RemoveCounters(CounterType.Loyality, 3);
          p.Effect = () => new Effects.ReturnToHand();
          p.TargetSelector.AddEffect(trg => trg.Card(c => !c.Is().Land, canTargetSelf: false).On.Battlefield());
          p.TargetingRule(new EffectBounce());
          p.TimingRule(new OnFirstMain());
          p.ActivateAsSorcery = true;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{-8}: Each player shuffles their hand and graveyard into their library. You draw seven cards.";
          p.Cost = new RemoveCounters(CounterType.Loyality, 8);
          p.Effect = () => new EachPlayerShufflesHandAndGraveyardIntoLibraryAndDrawsCards(7, onlyYouDraw: true);
          p.TimingRule(new OnFirstMain());
          p.ActivateAsSorcery = true;
        });
    }
  }
}