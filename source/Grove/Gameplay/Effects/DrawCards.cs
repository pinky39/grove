namespace Grove.Gameplay.Effects
{
  public class DrawCards : Effect
  {
    private readonly DynParam<int> _count;
    private readonly int _discardCount;
    private readonly int _lifeloss;
    private readonly DynParam<Player> _player;

    private DrawCards() {}

    public DrawCards(DynParam<int> count, int discardCount = 0, int lifeloss = 0,
      DynParam<Player> player = null)
    {
      _count = count;
      _discardCount = discardCount;
      _lifeloss = lifeloss;
      _player = player;

      RegisterDynamicParameters(count, player);
    }

    protected override void ResolveEffect()
    {
      Player player = _player ?? Controller;

      player.DrawCards(_count.Value);

      if (_lifeloss > 0)
        player.Life -= _lifeloss;

      if (_discardCount > 0)
      {
        Enqueue(new Decisions.DiscardCards(
          player,
          p => p.Count = _discardCount));
      }
    }
  }
}