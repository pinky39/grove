namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class OnLifeChanged : Trigger, IReceive<LifeChangedEvent>
  {
    private readonly Func<FilterParameters, bool> _filter;

    private OnLifeChanged() {}

    public OnLifeChanged(Func<FilterParameters, bool> filter)
    {
      _filter = filter;
    }

    public void Receive(LifeChangedEvent message)
    {
      if (_filter(new FilterParameters(message, this)))
      {
        Set(message);
      }
    }

    public class FilterParameters
    {
      private readonly LifeChangedEvent _message;
      private readonly Trigger _trigger;

      public FilterParameters(LifeChangedEvent message, Trigger trigger)
      {
        _message = message;
        _trigger = trigger;
      }

      public bool IsYours
      {
        get { return _message.Player == _trigger.Controller; }
      }

      public bool IsOpponents
      {
        get { return !IsYours; }
      }

      public bool IsGain
      {
        get { return _message.IsLifeGain; }
      }

      public bool IsLoss
      {
        get { return _message.IsLifeLoss; }
      }
    }
  }
}