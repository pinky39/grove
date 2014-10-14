namespace Grove.Effects
{
  public class PlayerPutsTopCardsFromLibraryToGraveyard : Effect
  {
    private readonly DynParam<int> _count;
    private readonly DynParam<Player> _player;

    private PlayerPutsTopCardsFromLibraryToGraveyard() {}

    public PlayerPutsTopCardsFromLibraryToGraveyard(DynParam<Player> player, DynParam<int> count)
    {
      _count = count;
      _player = player;

      RegisterDynamicParameters(player, count);
    }

    protected override void ResolveEffect()
    {
      var player = Target != null && Target.IsPlayer() 
        ? Target.Player() 
        : _player.Value;
      
      player.Mill(_count.Value);
    }
  }
}