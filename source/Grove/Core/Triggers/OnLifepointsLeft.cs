namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class OnLifepointsLeft : Trigger, IReceive<PlayerLifeChanged>
  {
    private readonly Func<TriggeredAbility, bool> _predicate;

    private OnLifepointsLeft() {}

    public OnLifepointsLeft(Func<TriggeredAbility, bool> predicate)
    {
      _predicate = predicate;
    }

    public void Receive(PlayerLifeChanged message)
    {
      if (_predicate(Ability))
      {
        Set();
      }
    }

    protected override void OnActivate()
    {
      if (_predicate(Ability))
      {
        Set();
      }
    }
  }
}