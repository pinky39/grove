namespace Grove.Effects
{
  using System.Linq;

  public class EachPlayerShufflesHandAndGraveyardIntoLibraryAndDrawsCards : Effect
  {
    private readonly int _count;
    private readonly bool _onlyYouDraw;

    private EachPlayerShufflesHandAndGraveyardIntoLibraryAndDrawsCards() {}

    public EachPlayerShufflesHandAndGraveyardIntoLibraryAndDrawsCards(int count, bool onlyYouDraw = false)
    {
      _count = count;
      _onlyYouDraw = onlyYouDraw;
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

      if (!_onlyYouDraw || player == Controller)
      {
        player.DrawCards(_count);
      }  
    }
  }
}