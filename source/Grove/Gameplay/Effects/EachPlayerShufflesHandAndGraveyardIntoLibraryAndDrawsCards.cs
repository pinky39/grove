namespace Grove.Gameplay.Effects
{
  using System.Linq;

  public class EachPlayerShufflesHandAndGraveyardIntoLibraryAndDrawsCards : Effect
  {
    private readonly int _count;

    private EachPlayerShufflesHandAndGraveyardIntoLibraryAndDrawsCards() {}

    public EachPlayerShufflesHandAndGraveyardIntoLibraryAndDrawsCards(int count)
    {
      _count = count;
    }

    protected override void ResolveEffect()
    {
      ShuffleIntoLibrary(Players.Active);
      ShuffleIntoLibrary(Players.Passive);
    }

    private void ShuffleIntoLibrary(Player player)
    {
      foreach (var card in player.Hand.ToList())
      {
        player.PutOnBottomOfLibrary(card);
      }

      foreach (var card in player.Graveyard.ToList())
      {
        player.PutOnBottomOfLibrary(card);
      }

      player.ShuffleLibrary();

      player.DrawCards(_count);
    }
  }
}