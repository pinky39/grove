namespace Grove.Ui.Zones
{
  using System.Collections.Generic;
  using Castle.Core;
  using Core;
  using Infrastructure;

  [Transient]
  public class ViewModel
  {
    private readonly Publisher _publisher;

    public ViewModel(Players players, Publisher publisher, Hand.ViewModel.IFactory handViewModelFactory)
    {
      _publisher = publisher;

      OpponentsHand = handViewModelFactory.Create(players.Computer.Hand);
      YourHand = handViewModelFactory.Create(players.Human.Hand);

      OpponentsGraveyard = players.Computer.Graveyard;
      YourGraveyard = players.Human.Graveyard;
    }

    public IEnumerable<Card> OpponentsGraveyard { get; private set; }
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

    public IEnumerable<Card> YourGraveyard { get; private set; }
    public Hand.ViewModel YourHand { get; private set; }

    public void ChangePlayersInterest(object cardOrAbility)
    {
      _publisher.Publish(new PlayersInterestChanged{
        Visual = cardOrAbility
      });
    }
  }
}