namespace Grove.UserInterface.CardActivation
{
  using Gameplay.Targeting;
  using Messages;

  public class ViewModel : ViewModelBase
  {
    public ViewModel(object activation)
    {
      Activation = activation;
    }

    public object Activation { get; private set; }

    public string Message { get { return Activation.ToString(); } }

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

      Shell.Publish(message);
    }

    public interface IFactory
    {
      ViewModel Create(object activation);
    }
  }
}