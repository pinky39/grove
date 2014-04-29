namespace Grove.UserInterface.CardActivation
{
  using Events;
  using Messages;

  public class ViewModel : ViewModelBase
  {
    public ViewModel(ICardActivationEvent activation)
    {
      Activation = activation;
    }

    public ICardActivationEvent Activation { get; private set; }
    public string Title { get { return Activation.GetTitle(); } }

    public void ChangePlayersInterestTarget(ITarget target, bool hasLostInterest)
    {
      if (target.IsPlayer())
        return;

      var card = target.IsCard() ? target.Card() : target.Effect().Source.OwningCard;

      var message = new PlayersInterestChanged
        {
          Visual = card,
          HasLostInterest = hasLostInterest,
        };

      Publisher.Publish(message);
    }

    public interface IFactory
    {
      ViewModel Create(ICardActivationEvent activation);
    }
  }
}