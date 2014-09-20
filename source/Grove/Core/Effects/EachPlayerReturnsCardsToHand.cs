namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Decisions;

  public class EachPlayerReturnsCardsToHand : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly bool _aiOrdersByDescendingScore;
    private readonly Func<Card, bool> _filter;
    private readonly int _maxCount;
    private readonly int _minCount;
    private readonly string _text;
    private readonly Zone _zone;

    private EachPlayerReturnsCardsToHand() {}

    public EachPlayerReturnsCardsToHand(int minCount, int maxCount, Zone zone,
      bool aiOrdersByDescendingScore, string text, Func<Card, bool> filter = null)
    {
      _minCount = minCount;
      _text = text;
      _aiOrdersByDescendingScore = aiOrdersByDescendingScore;
      _zone = zone;
      _filter = filter ?? delegate { return true; };
      _maxCount = maxCount;
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      if (_aiOrdersByDescendingScore)
      {
        return candidates
          .OrderBy(x => -x.Score)
          .Take(_minCount)
          .ToList();
      }

      return candidates
        .OrderBy(x => x.Score)
        .Take(_maxCount)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.PutToHand();
      }
    }

    protected override void ResolveEffect()
    {
      ReturnCardToHand(Players.Active);
      ReturnCardToHand(Players.Passive);
    }

    private void ReturnCardToHand(Player player)
    {
      Enqueue(new SelectCards(
        player,
        p =>
          {
            p.MinCount = _minCount;
            p.MaxCount = _maxCount;
            p.SetValidator(_filter);
            p.Zone = _zone;
            p.Text = _text;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }));
    }
  }
}