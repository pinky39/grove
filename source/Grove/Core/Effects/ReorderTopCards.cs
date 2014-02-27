namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.AI;
  using Grove.Decisions;

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
      return QuickDecisions.OrderTopCards(candidates, Controller);
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
  }
}