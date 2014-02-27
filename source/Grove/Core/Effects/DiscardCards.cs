namespace Grove.Effects
{
  public class DiscardCards : Effect
  {
    private readonly int _count;
    private readonly DynParam<Player> _player;

    private DiscardCards() {}

    public DiscardCards(int count, DynParam<Player> player = null)
    {
      _count = count;
      _player = player;

      RegisterDynamicParameters(player);
    }

    protected override void ResolveEffect()
    {
      var player = _player ?? Target.Player();

      Enqueue(new Decisions.DiscardCards(
        player.Value,
        p => p.Count = _count));
    }
  }
}