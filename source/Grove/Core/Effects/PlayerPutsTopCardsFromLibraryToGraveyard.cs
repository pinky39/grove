namespace Grove.Effects
{
  public class PlayerPutsTopCardsFromLibraryToGraveyard : Effect
  {
    private readonly int _count;
    private readonly DynParam<Player> _player;

    private PlayerPutsTopCardsFromLibraryToGraveyard() {}

    public PlayerPutsTopCardsFromLibraryToGraveyard(DynParam<Player> player, int count)
    {
      _count = count;
      _player = player;

      RegisterDynamicParameters(player);
    }

    protected override void ResolveEffect()
    {
      _player.Value.Mill(_count);
    }
  }
}