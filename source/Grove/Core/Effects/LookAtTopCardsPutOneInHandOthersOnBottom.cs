namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.AI;
  using Grove.Decisions;
  using Grove.Infrastructure;

  public class LookAtTopCardsPutOneInHandOthersOnBottom : Effect, IProcessDecisionResults<Ordering>,
    IChooseDecisionResults<List<Card>, Ordering>
  {
    private readonly int _count;

    private LookAtTopCardsPutOneInHandOthersOnBottom() {}

    public LookAtTopCardsPutOneInHandOthersOnBottom(int count)
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

      if (cards.Count > 0)
      {
        Controller.PutCardToHand(cards[0]);

        foreach (var card in cards.Skip(1))
        {
          Controller.PutOnBottomOfLibrary(card);
        }
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