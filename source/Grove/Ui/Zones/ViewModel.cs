namespace Grove.Ui.Zones
{
  using Core;

  public class ViewModel
  {
    public ViewModel(
      Players players,
      Hand.ViewModel.IFactory handViewModelFactory,
      Graveyard.ViewModel.IFactory graveyardViewModel)
    {
      OpponentsHand = handViewModelFactory.Create(players.Computer.Hand);
      YourHand = handViewModelFactory.Create(players.Human.Hand);

      OpponentsGraveyard = graveyardViewModel.Create(players.Computer.Graveyard);
      YourGraveyard = graveyardViewModel.Create(players.Human.Graveyard);
    }

    public Graveyard.ViewModel OpponentsGraveyard { get; private set; }
    public Hand.ViewModel OpponentsHand { get; private set; }
    public Graveyard.ViewModel YourGraveyard { get; private set; }
    public Hand.ViewModel YourHand { get; private set; }
  }
}