namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class UntapEachPermanent : Effect
  {
    private readonly Func<Card, bool> _filter;
    private readonly ControlledBy _controlledBy;

    private UntapEachPermanent() {}

    public UntapEachPermanent(Func<Card, bool> filter, ControlledBy controlledBy = ControlledBy.Any)
    {
      _filter = filter ?? delegate { return true; };
      _controlledBy = controlledBy;
    }

    protected override void ResolveEffect()
    {
      var cards = new List<Card>();

      if (_controlledBy == ControlledBy.Any)
      {
        cards.AddRange(Game.Players.Permanents().Where(_filter));
      }

      if (_controlledBy == ControlledBy.SpellOwner)
      {
        cards.AddRange(Controller.Battlefield.Where(_filter));
      }

      if (_controlledBy == ControlledBy.Opponent)
      {
        cards.AddRange(Controller.Opponent.Battlefield.Where(_filter));
      }

      foreach (var card in cards)
      {
        card.Untap();
      }
    }
  }
}
