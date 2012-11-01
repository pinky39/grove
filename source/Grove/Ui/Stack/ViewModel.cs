namespace Grove.Ui.Stack
{
  using System;
  using Core.Details.Cards.Effects;
  using Core.Targeting;
  using Core.Zones;
  using Infrastructure;

  public class ViewModel : IReceive<UiInteractionChanged>
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

    public void Receive(UiInteractionChanged message)
    {
      switch (message.State)
      {
        case (InteractionState.SelectTarget):
          {
            _select = ChangeSelection;
            break;
          }
        case (InteractionState.Disabled):
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

      _publisher.Publish(message);
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

    private void ChangeSelection(Effect effect)
    {
      _publisher.Publish(
        new SelectionChanged {Selection = effect});
    }
  }
}