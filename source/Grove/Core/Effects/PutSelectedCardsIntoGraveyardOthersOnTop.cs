namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Castle.Core.Internal;
  using Decisions;

  public class PutSelectedCardsIntoGraveyardOthersOnTop : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>, IProcessDecisionResults<Ordering>, IChooseDecisionResults<List<Card>, Ordering>
  {
    private readonly int _count;

    private PutSelectedCardsIntoGraveyardOthersOnTop() {}

    public PutSelectedCardsIntoGraveyardOthersOnTop(int count)
    {
      _count = count;
    }

    protected override void ResolveEffect()
    {
      var cards = Controller.Library
        .Take(_count)
        .ToList();

      foreach (var card in cards)
      {
        card.Peek();
      }

      Enqueue(new SelectCards(Controller,
        p =>
        {
          p.SetValidator(c => cards.Contains(c));
          p.Zone = Zone.Library;
          p.MinCount = 0;
          p.MaxCount = null;
          p.Text = "Select any number of cards to put them into graveyard.";
          p.OwningCard = Source.OwningCard;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
        }
        ));
    }

    ChosenCards IChooseDecisionResults<List<Card>, ChosenCards>.ChooseResult(List<Card> candidates)
    {
      var landsInPlay = Controller.Battlefield.Lands.Count();

      var needsLands = !Controller.Hand.Lands.Any() &&
        landsInPlay <= 5;

      // Select cards to be put into graveyard
      return candidates
        .Where(x =>
        {
          if (x.Is().Land)
          {
            return !needsLands;
          }

          // If card cannot be played return true (it will be put into graveyard)
          return x.ConvertedCost > landsInPlay;
        })
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.PutToGraveyard();
      }

      if (results.Count == _count)
        return;

      // Put the rest cards back on top library in any order
      var cards = Controller.Library
        .Take(_count - results.Count)
        .ToList();

      Enqueue(new OrderCards(
        Controller,
        p =>
        {
          p.Cards = cards;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.Title = "Order cards from top to bottom";
        }));
    }

    Ordering IChooseDecisionResults<List<Card>, Ordering>.ChooseResult(List<Card> candidates)
    {
      return QuickDecisions.OrderTopCards(candidates, Controller);
    }

    public void ProcessResults(Ordering result)
    {
      Controller.ReorderTopCardsOfLibrary(result.Indices);
    }
  }
}
