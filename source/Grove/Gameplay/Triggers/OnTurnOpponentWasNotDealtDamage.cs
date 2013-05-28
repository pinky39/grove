namespace Grove.Gameplay.Triggers
{
  using System;
  using Abilities;
  using Infrastructure;
  using Messages;
  using States;
  using Zones;

  [Serializable]
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
        if (_wasDealtDamage == false && IsOwningCardInPlay())
          Set();

        _wasDealtDamage.Value = false;
      }
    }

    private bool IsOwningCardInPlay()
    {
      return Ability.OwningCard.Zone == Zone.Battlefield;
    }

    public override void Initialize(TriggeredAbility triggeredAbility, Game game)
    {
      base.Initialize(triggeredAbility, game);
      _wasDealtDamage.Initialize(game.ChangeTracker);
    }
  }
}