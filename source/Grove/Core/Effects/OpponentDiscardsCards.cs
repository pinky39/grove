namespace Grove.Core.Effects
{
  using System;

  public class OpponentDiscardsCards : Effect
  {
    private readonly Func<Card, bool> _filter;
    private readonly int _randomCount;
    private readonly int _selectedCount;
    private readonly bool _youChooseDiscardedCards;

    public OpponentDiscardsCards(int randomCount = 0, int selectedCount = 0, bool youChooseDiscardedCards = false,
      Func<Card, bool> filter = null)
    {
      _randomCount = randomCount;
      _filter = filter ?? delegate { return true; };
      _youChooseDiscardedCards = youChooseDiscardedCards;
      _selectedCount = selectedCount;
    }

    protected override void ResolveEffect()
    {
      var opponent = Players.GetOpponent(Controller);

      if (_youChooseDiscardedCards)
      {
        opponent.RevealHand();

        Enqueue<Decisions.DiscardCards>(
          controller: Controller,
          init: p =>
            {
              p.Count = _selectedCount;
              p.Filter = _filter;
              p.DiscardOpponentsCards = true;
            });

        return;
      }

      for (var i = 0; i < _randomCount; i++)
      {
        opponent.DiscardRandomCard();
      }

      if (_selectedCount == 0)
        return;

      Enqueue<Decisions.DiscardCards>(
        controller: opponent,
        init: p =>
          {
            p.Count = _selectedCount;
            p.Filter = _filter;
          });
    }
  }
}