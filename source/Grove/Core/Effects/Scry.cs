namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Decisions;

  public class Scry : Effect, IProcessDecisionResults<Ordering>,
    IChooseDecisionResults<List<Card>, Ordering>
  {
    private readonly int _count;

    private Scry() {}

    public Scry(int count)
    {
      _count = count;
    }

    public Ordering ChooseResult(List<Card> candidates)
    {
      return QuickDecisions.OrderTopAndBottomCards(candidates, Controller);
    }

    public void ProcessResults(Ordering result)
    {
      Controller.ReorderTopAndBottomCardsOfLibrary(result.Indices);
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

      Enqueue(new OrderTopAndBottomCards(
        Controller,
        p =>
        {
          p.Cards = cards;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.Title = "Order any number of cards to put them on top and the rest on bottom (from top to bottom).";
        }));
    }
  }
}
