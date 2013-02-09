namespace Grove.Core.Effects
{
  using System;

  public class OpponentDiscardsCards : Effect
  {
    private readonly Func<Card, bool> _filter;
    private readonly Func<Effect, int> _getRandomCount;
    private readonly Func<Effect, int> _getSelectedCount;
    private readonly bool _youChooseDiscardedCards;

    private OpponentDiscardsCards() {}

    public OpponentDiscardsCards(Func<Effect, int> randomCount = null, Func<Effect, int> selectedCount = null,
      bool youChooseDiscardedCards = false, Func<Card, bool> filter = null)
    {
      _getRandomCount = randomCount ?? (e => 0);
      _filter = filter ?? delegate { return true; };
      _youChooseDiscardedCards = youChooseDiscardedCards;
      _getSelectedCount = selectedCount ?? (e => 0);
    }

    public OpponentDiscardsCards(int randomCount = 0, int selectedCount = 0, bool youChooseDiscardedCards = false,
      Func<Card, bool> filter = null) : this(e => randomCount, e => selectedCount, youChooseDiscardedCards, filter) {}

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
              p.Count = _getSelectedCount(this);
              p.Filter = _filter;
              p.DiscardOpponentsCards = true;
            });

        return;
      }

      for (var i = 0; i < _getRandomCount(this); i++)
      {
        opponent.DiscardRandomCard();
      }

      if (_getSelectedCount(this) == 0)
        return;

      Enqueue<Decisions.DiscardCards>(
        controller: opponent,
        init: p =>
          {
            p.Count = _getSelectedCount(this);
            p.Filter = _filter;
          });
    }
  }
}