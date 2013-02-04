namespace Grove.Core.Effects
{
  using System;

  public class DrawCards : Effect
  {
    private readonly Func<Effect, int> _count;
    private readonly int _discardCount;
    private readonly int _lifeloss;

    private DrawCards() {}

    public DrawCards(Func<Effect, int> count, int discardCount = 0, int lifeloss = 0)
    {
      _count = count;
      _discardCount = discardCount;
      _lifeloss = lifeloss;
    }

    public DrawCards(int count, int discardCount = 0, int lifeloss = 0)
      : this(delegate { return count; }, discardCount, lifeloss) {}

    protected override void ResolveEffect()
    {
      Controller.DrawCards(_count(this));

      if (_lifeloss > 0)
        Controller.Life -= _lifeloss;

      if (_discardCount > 0)
        Game.Enqueue<Decisions.DiscardCards>(
          controller: Controller,
          init: p => p.Count = _discardCount);
    }
  }
}