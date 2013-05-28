namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;

  [Serializable]
  public class ReorderTopCards : Effect, IProcessDecisionResults<Ordering>, IChooseDecisionResults<List<Card>, Ordering>
  {
    private readonly int _count;

    private ReorderTopCards() {}

    public ReorderTopCards(int count)
    {
      _count = count;
    }

    public Ordering ChooseResult(List<Card> candidates)
    {
      var landsInPlay = Controller.Battlefield.Lands.Count();

      var needsLands = Controller.Hand.Lands.Count() == 0 &&
        landsInPlay <= 5;

      var indices = candidates.Select((x, i) =>
        {
          int score;

          if (x.Is().Land)
          {
            score = needsLands ? 100 : 0;
          }
          else if (x.ConvertedCost < landsInPlay)
          {
            score = x.ConvertedCost;
          }
          else
          {
            score = -x.ConvertedCost;
          }

          return new
            {
              Card = x,
              Index = i,
              Score = score
            };
        })
        .OrderByDescending(x => x.Score)
        .Select(x => x.Index)
        .ToArray();

      return new Ordering(indices);
    }

    public void ProcessResults(Ordering result)
    {
      Controller.ReorderTopCardsOfLibrary(result.Indices);      
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

      Enqueue<OrderCards>(
        controller: Controller,
        init: p =>
          {
            p.Cards = cards;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
            p.Title = "Order the cards from top to bottom";
          });
    }
  }
}