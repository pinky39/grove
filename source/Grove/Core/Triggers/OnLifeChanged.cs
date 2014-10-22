namespace Grove.Triggers
{
  using Events;
  using Infrastructure;

  public class OnLifeChanged : Trigger, IReceive<LifeChangedEvent>
  {
    private readonly bool _isGain;
    private readonly bool _isLoss;
    private readonly bool _isOpponents;
    private readonly bool _isYours;

    private OnLifeChanged() {}

    public OnLifeChanged(bool isYours = false, bool isOpponents = false, bool isGain = false, bool isLoss = false)
    {
      _isYours = isYours;
      _isOpponents = isOpponents;
      _isGain = isGain;
      _isLoss = isLoss;
    }

    public void Receive(LifeChangedEvent message)
    {
      if (_isYours && message.Player == Controller)
      {
        CheckGainLoss(message);
      }
      else if (_isOpponents && message.Player != Controller)
      {
        CheckGainLoss(message);
      }
    }

    private void CheckGainLoss(LifeChangedEvent message)
    {
      if (_isGain && message.IsLifeGain)
      {
        Set(message);
      }
      else if (_isLoss && message.IsLifeLoss)
      {
        Set(message);
      }
    }
  }
}