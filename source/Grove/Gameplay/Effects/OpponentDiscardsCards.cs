namespace Grove.Gameplay.Effects
{
  using System;

  [Serializable]
  public class OpponentDiscardsCards : Effect
  {
    private readonly Func<Card, bool> _filter;
    private readonly DynParam<int> _randomCount;
    private readonly DynParam<int> _selectedCount;
    private readonly bool _youChooseDiscardedCards;

    private OpponentDiscardsCards() {}

    public OpponentDiscardsCards(DynParam<int> randomCount = null, DynParam<int> selectedCount = null,
      bool youChooseDiscardedCards = false, Func<Card, bool> filter = null)
    {
      _randomCount = randomCount ?? 0;
      _selectedCount = selectedCount ?? 0;

      _filter = filter ?? delegate { return true; };
      _youChooseDiscardedCards = youChooseDiscardedCards;

      RegisterDynamicParameters(randomCount, selectedCount);
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
              p.Count = _selectedCount.Value;
              p.Filter = _filter;
              p.DiscardOpponentsCards = true;
            });

        return;
      }

      for (var i = 0; i < _randomCount.Value; i++)
      {
        opponent.DiscardRandomCard();
      }

      if (_selectedCount.Value == 0)
        return;

      Enqueue<Decisions.DiscardCards>(
        controller: opponent,
        init: p =>
          {
            p.Count = _selectedCount.Value;
            p.Filter = _filter;
          });
    }
  }
}