namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using AI;
  using Decisions;
  using Infrastructure;
  using Messages;

  public class SearchLibraryPutToZone : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>, ICardValidator
  {
    private readonly Action<Card> _afterPutToZone;
    private readonly int _maxCount;
    private readonly int _minCount;
    private readonly DynParam<Player> _player;
    private readonly bool _revealCards;

    private readonly string _text;
    private readonly Func<Effect, Card, bool> _validator;
    private readonly Zone _zone;

    private SearchLibraryPutToZone() {}

    public SearchLibraryPutToZone(Zone zone, Action<Card> afterPutToZone = null,
      int maxCount = 1, int minCount = 0, Func<Effect, Card, bool> validator = null,
      string text = null, bool revealCards = true, DynParam<Player> player = null)
    {
      _validator = validator ?? delegate { return true; };
      _player = player ?? new DynParam<Player>((e, g) => e.Controller, evaluateOnResolve: true);
      _text = text ?? "Search your library for a card.";
      _zone = zone;
      _afterPutToZone = afterPutToZone ?? delegate { };
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
      return CardPicker
        .ChooseBestCards(
          candidates,
          _maxCount,
          aurasNeedTarget: _zone == Zone.Battlefield);
    }

    public void ProcessResults(ChosenCards results)
    {
      var i = 0;

      while (i < results.Count)
      {
        var card = results[i++];
        Card attachTo = null;

        if (_zone == Zone.Battlefield && card.Is().Aura)
        {
          attachTo = results[i++];
        }

        PutToZone(card, attachTo);

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

    private void PutToZone(Card card, Card attachTo)
    {
      switch (_zone)
      {
        case (Zone.Hand):
          {
            card.PutToHandFrom(Zone.Library);
            break;
          }
        case (Zone.Battlefield):
          {
            if (attachTo == null)
            {
              card.PutToBattlefield();
              break;
            }

            card.EnchantWithoutPayingCost(attachTo);
            break;
          }
        case (Zone.Graveyard):
          {
            card.PutToGraveyard();
            break;
          }
        default:
          {
            Asrt.Fail(
              String.Format("Zone not supported: {0}.", _zone));
            break;
          }
      }

      _afterPutToZone(card);
    }

    protected override void ResolveEffect()
    {
      _player.Value.RevealLibrary();

      Enqueue(new SelectCards(
        _player.Value,
        p =>
          {
            p.MinCount = _minCount;
            p.MaxCount = _maxCount;
            p.Validator = this;
            p.Zone = Zone.Library;
            p.Text = _text;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
            p.OwningCard = Source.OwningCard;
            p.AurasNeedTarget = _zone == Zone.Battlefield;
          }));
    }
  }
}