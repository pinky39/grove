namespace Grove.Core.Effects
{
  using System;

  public class DrawCards : Effect
  {
    private readonly DynamicParameter<int> _count;
    private readonly int _discardCount;
    private readonly int _lifeloss;
    private readonly DynamicParameter<Player> _player;

    private DrawCards() {}

    public DrawCards(Func<Effect, int> count, int discardCount = 0, int lifeloss = 0,
      Func<Effect, Player> player = null)
    {
      _count = count;
      _player = player ?? (e => e.Controller);
      _discardCount = discardCount;
      _lifeloss = lifeloss;
    }

    public DrawCards(int count, int discardCount = 0, int lifeloss = 0, Func<Effect, Player> player = null)
      : this(e => count, discardCount, lifeloss, player) {}

    protected override void Initialize()
    {
      _count.Evaluate(this);
      _player.Evaluate(this);
    }

    protected override void ResolveEffect()
    {
      var player = _player.Value;

      player.DrawCards(_count.Value);

      if (_lifeloss > 0)
        player.Life -= _lifeloss;

      if (_discardCount > 0)
        Enqueue<Decisions.DiscardCards>(
          controller: player,
          init: p => p.Count = _discardCount);
    }
  }
}