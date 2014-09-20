namespace Grove.UserInterface.Zones
{
  using System;

  public class ViewModel : ViewModelBase, IDisposable
  {
    public Graveyard.ViewModel OpponentsGraveyard { get; private set; }
    public Hand.ViewModel OpponentsHand { get; private set; }
    public Graveyard.ViewModel YourGraveyard { get; private set; }
    public Hand.ViewModel YourHand { get; private set; }
    public Library.ViewModel YourLibrary { get; private set; }
    public Library.ViewModel OpponentsLibrary { get; private set; }

    public override void Initialize()
    {
      OpponentsHand = ViewModels.Hand.Create(Players.Computer);
      YourHand = ViewModels.Hand.Create(Players.Human);

      OpponentsGraveyard = ViewModels.Graveyard.Create(Players.Computer);
      YourGraveyard = ViewModels.Graveyard.Create(Players.Human);

      OpponentsLibrary = ViewModels.Library.Create(Players.Computer);
      YourLibrary = ViewModels.Library.Create(Players.Human);
    }

    public void Dispose()
    {
      YourHand.Dispose();
      OpponentsHand.Dispose();
      YourGraveyard.Dispose();
      OpponentsGraveyard.Dispose();
      YourLibrary.Dispose();
      OpponentsLibrary.Dispose();
    }
  }
}