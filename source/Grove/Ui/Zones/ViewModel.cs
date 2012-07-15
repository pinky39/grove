namespace Grove.Ui.Zones
{
  using Core;
  using Infrastructure;

  public class ViewModel
  {
    private readonly Publisher _publisher;

    public ViewModel(Players players, Publisher publisher,
                     Hand.ViewModel.IFactory handViewModelFactory, Graveyard.ViewModel.IFactory graveyardViewModel)
    {
      _publisher = publisher;

      OpponentsHand = handViewModelFactory.Create(players.Computer.Hand);
      YourHand = handViewModelFactory.Create(players.Human.Hand);

      OpponentsGraveyard = graveyardViewModel.Create(players.Computer);
      YourGraveyard = graveyardViewModel.Create(players.Human);
    }

    public Graveyard.ViewModel OpponentsGraveyard { get; private set; }
    public Hand.ViewModel OpponentsHand { get; private set; }

    public bool ShowOpponentsHand
    {
      get
      {
#if DEBUG
        return true;
#else        
        return false;
#endif
      }
    }

    public Graveyard.ViewModel YourGraveyard { get; private set; }
    public Hand.ViewModel YourHand { get; private set; }
  }
}