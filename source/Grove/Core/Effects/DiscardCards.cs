namespace Grove.Core.Effects
{
  using Targeting;

  public class DiscardCards : Effect
  {
    private readonly int _count;

    public DiscardCards(int count)
    {
      _count = count;
    }

    protected override void ResolveEffect()
    {
      Game.Enqueue<Decisions.DiscardCards>(
        controller: Target.Player(),
        init: p => p.Count = _count);
    }
  }
}