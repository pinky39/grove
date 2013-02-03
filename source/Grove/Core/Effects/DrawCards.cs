namespace Grove.Core.Effects
{
  public class DrawCards : Effect
  {
    private readonly int _count;
    private readonly int _discardCount;
    private readonly int _lifeloss;

    public DrawCards(int count, int discardCount = 0, int lifeloss = 0)
    {
      _count = count;
      _discardCount = discardCount;
      _lifeloss = lifeloss;
    }

    protected override void ResolveEffect()
    {
      Controller.DrawCards(_count);

      if (_lifeloss > 0)
        Controller.Life -= _lifeloss;

      if (_discardCount > 0)
        Game.Enqueue<Decisions.DiscardCards>(
          controller: Controller,
          init: p => p.Count = _discardCount);
    }
  }
}