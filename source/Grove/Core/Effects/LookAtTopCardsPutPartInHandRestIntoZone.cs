namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Decisions;
  using Infrastructure;

  public abstract class LookAtTopCardsPutPartInHandRestIntoZone : Effect, IProcessDecisionResults<Ordering>,
    IChooseDecisionResults<List<Card>, Ordering>
  {
    private readonly int _count;
    private readonly int _toHandAmount;
    private readonly string _title;

    protected LookAtTopCardsPutPartInHandRestIntoZone() {}

    protected LookAtTopCardsPutPartInHandRestIntoZone(int count, int toHandAmount = 1)
    {
      _count = count;
      _toHandAmount = toHandAmount;

      _title = _toHandAmount > 1 
        ? "Order cards (first " + _toHandAmount + " cards go to your hand)" 
        : "Order cards (first card goes to your hand)";
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

      for (int i = 0; i < cards.Count; i++)
      {
        if (i < _toHandAmount)
        {
          Controller.PutCardToHand(cards[i]);
        }
        else
        {
          PutCardIntoZone(cards[i]);
        }
      }
    }

    protected abstract void PutCardIntoZone(Card card);

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
            p.Title = _title;
          }));
    }
  }
}
