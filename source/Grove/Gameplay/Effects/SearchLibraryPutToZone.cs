namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Messages;
  using Zones;

  public class SearchLibraryPutToZone : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>, ICardValidator
  {
    private readonly int _maxCount;
    private readonly int _minCount;
    private readonly Action<Card> _putToZone;
    private readonly bool _revealCards;
    private readonly DynParam<Player> _player;

    private readonly string _text;
    private readonly Func<Effect, Card, bool> _validator;

    private SearchLibraryPutToZone() {}

    public SearchLibraryPutToZone(Action<Card> putToZone, int maxCount = 1, int minCount = 0,
      Func<Effect, Card, bool> validator = null, string text = null, bool revealCards = true,
      DynParam<Player> player = null)
    {
      _validator = validator ?? delegate { return true; };
      _player = player ?? new DynParam<Player>((e, g) => e.Controller, evaluateOnResolve: true);
      _text = text ?? "Search your library for a card.";
      _putToZone = putToZone;
      _revealCards = revealCards;      
      _maxCount = maxCount;
      _minCount = minCount;

      RegisterDynamicParameters(_player);
    }

    public bool IsValidCard(Card card)
    {
      return _validator(this, card);
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => -x.Score)
        .Take(_maxCount)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        _putToZone(card);

        if (_revealCards)
        {
          Publish(new CardWasRevealed {Card = card});
        }
        else
        {
          card.ResetVisibility();
        }
      }

      Controller.ShuffleLibrary();
    }

    protected override void ResolveEffect()
    {            
      _player.Value.RevealLibrary();

      Enqueue<SelectCards>(
        controller: _player.Value,
        init: p =>
          {
            p.MinCount = _minCount;
            p.MaxCount = _maxCount;
            p.Validator(this);
            p.Zone = Zone.Library;
            p.Text = _text;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
            p.OwningCard = Source.OwningCard;
          });
    }
  }
}