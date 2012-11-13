namespace Grove.Ui.Stack
{
  using System;
  using Core;
  using Core.Cards.Effects;
  using Core.Targeting;
  using Core.Zones;
  using Infrastructure;

  public class ViewModel : IReceive<UiInteractionChanged>
  {
    private readonly Game _game;
    private Action<Effect> _select = delegate { };

    public ViewModel(Game game)
    {
      _game = game;
    }

    public Stack Stack { get { return _game.Stack; } }

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

      _game.Publish(message);
    }

    public void ChangePlayersInterest(Effect effect, bool hasLostInterest)
    {
      var message = new PlayersInterestChanged
        {
          Visual = effect.Source,
          HasLostInterest = hasLostInterest,
          Target = effect.Target()
        };

      _game.Publish(message);
    }

    private void ChangeSelection(Effect effect)
    {
      _game.Publish(
        new SelectionChanged {Selection = effect});
    }
  }
}