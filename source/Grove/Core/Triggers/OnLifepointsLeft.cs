namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class OnLifepointsLeft : Trigger, IReceive<LifeChangedEvent>
  {
    private readonly Func<TriggeredAbility, bool> _predicate;

    private OnLifepointsLeft() {}

    public OnLifepointsLeft(Func<TriggeredAbility, bool> predicate)
    {
      _predicate = predicate;
    }

    public void Receive(LifeChangedEvent message)
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