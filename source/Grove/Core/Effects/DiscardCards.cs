namespace Grove.Core.Effects
{
  using System;
  using Targeting;

  public class DiscardCards : Effect
  {
    private readonly int _count;
    private readonly Func<Effect, Player> _playerSelector;

    public DiscardCards(int count, Func<Effect, Player> playerSelector = null)
    {
      _count = count;
      _playerSelector = playerSelector ?? (e => e.Target.Player());
    }

    protected override void ResolveEffect()
    {                  
      Game.Enqueue<Decisions.DiscardCards>(
        controller: _playerSelector(this),
        init: p => p.Count = _count);
    }
  }
}