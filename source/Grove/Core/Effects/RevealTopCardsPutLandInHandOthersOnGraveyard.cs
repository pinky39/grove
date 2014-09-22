using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grove.Core.Effects
{
    using AI;
    using Decisions;
    using Infrastructure;

    class RevealTopCardsPutLandInHandOthersOnGraveyard: Effect, IProcessDecisionResults<Ordering>, IChooseDecisionResults<List<Card>, Ordering>
  {
    private readonly int _count;

    private RevealTopCardsPutLandInHandOthersOnGraveyard() {}

    public RevealTopCardsPutLandInHandOthersOnGraveyard(int count)
    {
      _count = count;
    }

    public Ordering ChooseResult(List<Card> candidates)
    {
      return QuickDecisions.OrderTopCards(candidates, Controller);
    }

    public void ProcessResults(Ordering results)
    {
        var cards = Controller.Library
        .Take(_count)
        .ToList().ShuffleInPlace(results.Indices);

        var added = false;

        foreach (var card in cards)
        {
            card.Reveal();

            if (!added && card.Is().Land)
            {
                Controller.PutCardToHand(card);

                added = true;

                continue;
            }

            Controller.PutCardToGraveyard(card);
        }
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

      Enqueue(new OrderCards(
        Controller,
        p =>
          {
            p.Cards = cards;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
            p.Title = "Order cards (first card goes to your hand)";
          }));
    }
  }
}
