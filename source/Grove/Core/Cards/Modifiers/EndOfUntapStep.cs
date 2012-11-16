namespace Grove.Core.Cards.Modifiers
{
  using Infrastructure;
  using Messages;

  public class EndOfUntapStep : Lifetime, IReceive<StepFinished>
  {
    public Player OnlyDuringPlayersUntap { get; set; }
    
    public void Receive(StepFinished message)
    {
      if (message.Step == Step.Untap)
      {
        if (OnlyDuringPlayersUntap != null && OnlyDuringPlayersUntap != Game.Players.Active)
        {
          return;
        }

        End();

      }
        
    }
  }
}