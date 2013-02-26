namespace Grove.Core.Triggers
{
  using Infrastructure;
  using Messages;

  public class OnTurnOpponentWasNotDealtDamage : Trigger, IReceive<StepStarted>, IReceive<DamageHasBeenDealt>
  {
    private readonly Trackable<bool> _wasDealtDamage = new Trackable<bool>();

    public void Receive(DamageHasBeenDealt message)
    {
      if (message.Receiver == Players.Passive)
      {
        _wasDealtDamage.Value = true;
      }
    }

    public void Receive(StepStarted message)
    {
      if (message.Step == Step.EndOfTurn)
      {
        if (_wasDealtDamage == false)
          Set();

        _wasDealtDamage.Value = false;
      }
    }

    public override void Initialize(TriggeredAbility triggeredAbility, Game game)
    {
      base.Initialize(triggeredAbility, game);      
      _wasDealtDamage.Initialize(game.ChangeTracker);
    }
  }
}