namespace Grove.Core.Effects
{
  using System;
  using Decisions;
  using Decisions.Results;
  using Messages;
  using Zones;

  public class SearchLibraryPutToHand : Effect, IProcessDecisionResults<ChosenCards>
  {
    private readonly bool _discardRandomCardAfterwards;
    private readonly int _maxCount;
    private readonly int _minCount;
    private readonly bool _revealCards;
    private readonly string _text;
    private readonly Func<Card, bool> _validator;

    private SearchLibraryPutToHand() {}

    public SearchLibraryPutToHand(int maxCount = 1, int minCount = 0, Func<Card, bool> validator = null,
      string text = null, bool discardRandomCardAfterwards = false, bool revealCards = true)
    {
      _discardRandomCardAfterwards = discardRandomCardAfterwards;
      _validator = validator ?? delegate { return true; };
      _text = text ?? "Search your library for a card.";
      _revealCards = revealCards;
      _maxCount = maxCount;
      _minCount = minCount;
    }

    public void ProcessResults(ChosenCards results)
    {
      if (_revealCards)
      {
        foreach (var card in results)
        {
          Publish(new CardWasRevealed {Card = card});
        }
      }
      else
      {
        foreach (var card in results)
        {
          card.ResetVisibility();
        }
      }

      if (_discardRandomCardAfterwards)
      {
        Controller.DiscardRandomCard();
      }

      Controller.ShuffleLibrary();
    }

    protected override void ResolveEffect()
    {
      Controller.RevealLibrary();

      Enqueue<SelectCardsPutToHand>(
        controller: Controller,
        init: p =>
          {
            p.MinCount = _minCount;
            p.MaxCount = _maxCount;
            p.Validator = _validator;
            p.Zone = Zone.Library;
            p.Text = FormatText(_text);
            p.AiOrdersByDescendingScore = true;
            p.ProcessDecisionResults = this;
          });
    }
  }
}