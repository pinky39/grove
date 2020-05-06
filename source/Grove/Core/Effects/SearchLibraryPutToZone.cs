namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using AI;
  using Decisions;
  using Events;
  using Infrastructure;
  using System.Linq;

  public class SearchLibraryPutToZone : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>, ICardValidator
  {
    private readonly EffectAction<Card> _afterPutToZone;
    private readonly int _maxCount;
    private readonly int _minCount;
    private readonly DynParam<Player> _player;
    private readonly bool _revealCards;
    private readonly CardOrder _rankingAlgorithm;

    private readonly string _text;
    private readonly CardSelector _validator;
    private readonly Zone _zone;

    private SearchLibraryPutToZone() { }

    public SearchLibraryPutToZone(Zone zone, EffectAction<Card> afterPutToZone = null,
      int maxCount = 1, int minCount = 0, CardSelector validator = null,
      string text = null, bool revealCards = true, DynParam<Player> player = null,
      CardOrder rankingAlgorithm = null)
    {
      _validator = validator ?? delegate   { return true; };
      _player = player ?? new DynParam<Player>((e, g) => e.Controller, EvaluateAt.OnResolve);
      _text = text ?? "Search your library for a card.";
      _zone = zone;
      _afterPutToZone = afterPutToZone ?? delegate { };
      _revealCards = revealCards;
      _rankingAlgorithm = rankingAlgorithm ?? ((c, ctx) => -c.Score);
      _maxCount = maxCount;
      _minCount = minCount;

      RegisterDynamicParameters(_player);
    }

    public bool IsValidCard(Card card)
    {
      return _validator(card, Ctx);
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return CardPicker
        .ChooseBestCards(
          controller: _player.Value,
          candidates: candidates,
          count: _maxCount,
          aurasNeedTarget: _zone == Zone.Battlefield,
          rankingAlgorithm: c => _rankingAlgorithm(c, Ctx));
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
          card.Reveal();
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
        case (Zone.Exile):
          {
            card.Exile();
            break;
          }
        default:
          {
            Asrt.Fail(String.Format("Zone not supported: {0}.", _zone));
            break;
          }
      }

      _afterPutToZone(card, Ctx);
    }

    protected override void ResolveEffect()
    {
      _player.Value.PeekLibrary();      

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

    public static int ChooseLandToPutToBattlefield(Card card, Context ctx)
    {
      // set the colorless count to a big number so colors
      // are always considered first
      var counts = new[] {0, 0, 0, 0, 0, 1000};

      foreach (var color in ctx.You.GetAvailableMana())
      {
        foreach (var index in color.Indices)
        {
          counts[index]++;
        }
      }

      return card.ProducableManaColors.Select(index => counts[index]).Min();
    }
    
  }
}