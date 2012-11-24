namespace Grove.Ui.Zones
{
  using Core;

  public class ViewModel
  {
    public ViewModel(
      Game game,
      Hand.ViewModel.IFactory handViewModelFactory,
      Graveyard.ViewModel.IFactory graveyardViewModel,
      Library.ViewModel.IFactory libraryViewModel)
    {
      OpponentsHand = handViewModelFactory.Create(game.Players.Computer.Hand);
      YourHand = handViewModelFactory.Create(game.Players.Human.Hand);

      OpponentsGraveyard = graveyardViewModel.Create(game.Players.Computer.Graveyard);
      YourGraveyard = graveyardViewModel.Create(game.Players.Human.Graveyard);

      OpponentsLibrary = libraryViewModel.Create(game.Players.Computer.Library);
      YourLibrary = libraryViewModel.Create(game.Players.Human.Library);
    }

    public Graveyard.ViewModel OpponentsGraveyard { get; private set; }
    public Hand.ViewModel OpponentsHand { get; private set; }
    public Graveyard.ViewModel YourGraveyard { get; private set; }
    public Hand.ViewModel YourHand { get; private set; }
    public Library.ViewModel YourLibrary { get; private set; }
    public Library.ViewModel OpponentsLibrary { get; private set; }
  }
}