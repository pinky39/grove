namespace Grove.Ui.Stack
{
  using System;
  using Core.Details.Cards.Effects;
  using Core.Zones;
  using Infrastructure;

  public class ViewModel : IReceive<SelectionModeChanged>
  {
    private readonly Publisher _publisher;
    private readonly Stack _stack;
    private Action<Effect> _select = delegate { };

    public ViewModel(Stack stack, Publisher publisher)
    {
      _stack = stack;
      _publisher = publisher;
    }

    public Stack Stack { get { return _stack; } }

    public void Receive(SelectionModeChanged message)
    {
      switch (message.SelectionMode)
      {
        case (SelectionMode.SelectTarget):
          {
            _select = MarkAsTarget;
            break;
          }
        case (SelectionMode.Disabled):
          {
            _select = delegate { };
            break;
          }
      }
    }

    public void Select(Effect effect)
    {
      _select(effect);
    }

    public void ChangePlayersInterest(Effect effect, bool hasLostInterest)
    {
      var message = new PlayersInterestChanged
        {
          Visual = effect.Source,
          HasLostInterest = hasLostInterest,
          Target = effect.Target()
        };

      _publisher.Publish(message);
    }

    private void MarkAsTarget(Effect effect)
    {
      _publisher.Publish(
        new TargetSelected {Target = effect});
    }
  }
}