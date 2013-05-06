namespace Grove.UserInterface.MagnifiedCard
{
  using Infrastructure;

  public class ViewModel : IReceive<PlayersInterestChanged>
  {
    public virtual object Visual { get; protected set; }

    public void Receive(PlayersInterestChanged message)
    {
      if (message.HasLostInterest == false)
        Visual = message.Visual;
    }
  }
}