namespace Grove.Core.Effects
{
  using System;

  public class DrawCards : Effect
  {
    private readonly Func<Effect, int> _count;
    private readonly int _discardCount;
    private readonly Func<Effect, Player> _getPlayer;
    private readonly int _lifeloss;

    private DrawCards() {}

    public DrawCards(Func<Effect, int> count, int discardCount = 0, int lifeloss = 0,
      Func<Effect, Player> getPlayer = null)
    {
      _count = count;
      _getPlayer = getPlayer ?? (e => e.Controller);
      _discardCount = discardCount;
      _lifeloss = lifeloss;
    }

    public DrawCards(int count, int discardCount = 0, int lifeloss = 0, Func<Effect, Player> getPlayer = null)
      : this(delegate { return count; }, discardCount, lifeloss, getPlayer)
    {
      _getPlayer = getPlayer;
    }

    protected override void ResolveEffect()
    {
      var player = _getPlayer(this);

      player.DrawCards(_count(this));

      if (_lifeloss > 0)
        player.Life -= _lifeloss;

      if (_discardCount > 0)
        Game.Enqueue<Decisions.DiscardCards>(
          controller: player,
          init: p => p.Count = _discardCount);
    }
  }
}