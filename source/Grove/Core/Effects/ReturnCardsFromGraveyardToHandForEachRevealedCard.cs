namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;

  public class ReturnCardsFromGraveyardToHandForEachRevealedCard : Effect
  {
    private readonly Func<Card, bool> _graveyardFilter;
    private readonly Func<Card, bool> _revealFilter;

    private ReturnCardsFromGraveyardToHandForEachRevealedCard() {}

    public ReturnCardsFromGraveyardToHandForEachRevealedCard(Func<Card, bool> revealFilter,
      Func<Card, bool> graveyardFilter)
    {
      _revealFilter = revealFilter;
      _graveyardFilter = graveyardFilter;
    }

    protected override void ResolveEffect()
    {
      var handler = new RevealHandler(this);

      Enqueue(new SelectCards(Controller, p =>
        {
          p.SetValidator(_revealFilter);
          p.Zone = Zone.Hand;
          p.MinCount = 0;
          p.Text = "Choose any number of cards in your hand.";
          p.OwningCard = Source.OwningCard;
          p.ProcessDecisionResults = handler;
          p.ChooseDecisionResults = handler;
        }));
    }

    [Copyable]
    private class ReturnHandler : IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
    {
      private readonly int _count;
      private readonly ReturnCardsFromGraveyardToHandForEachRevealedCard _e;

      private ReturnHandler() {}

      public ReturnHandler(int count, ReturnCardsFromGraveyardToHandForEachRevealedCard e)
      {
        _count = count;
        _e = e;
      }

      public ChosenCards ChooseResult(List<Card> candidates)
      {
        return candidates
          .OrderByDescending(x => x.Score)
          .Take(_count)
          .ToList();
      }

      public void ProcessResults(ChosenCards results)
      {
        foreach (var card in results)
        {
          card.PutToHandFrom(Zone.Graveyard);
        }
      }
    }

    [Copyable]
    private class RevealHandler : IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
    {
      private readonly ReturnCardsFromGraveyardToHandForEachRevealedCard _e;

      private RevealHandler() {}

      public RevealHandler(ReturnCardsFromGraveyardToHandForEachRevealedCard e)
      {
        _e = e;
      }

      public ChosenCards ChooseResult(List<Card> candidates)
      {
        return candidates
          .Take(_e.Controller.Graveyard.Count(_e._graveyardFilter))
          .ToList();
      }

      public void ProcessResults(ChosenCards results)
      {
        foreach (var chosenCard in results)
        {
          chosenCard.Reveal();
        }

        var handler = new ReturnHandler(results.Count, _e);

        _e.Enqueue(new SelectCards(
          _e.Controller,
          p =>
            {
              p.MinCount = 0;
              p.MaxCount = results.Count;
              p.Text = String.Format("Select {0} card(s) to return to hand.", results.Count);
              p.SetValidator(_e._graveyardFilter);
              p.Zone = Zone.Graveyard;
              p.OwningCard = _e.Source.OwningCard;
              p.ProcessDecisionResults = handler;
              p.ChooseDecisionResults = handler;
            }
          ));
      }
    }
  }
}