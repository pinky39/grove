namespace Grove.Ui.Stack
{
  using System;
  using System.Collections.Generic;
  using Caliburn.Micro;
  using Core;
  using Core.Cards.Effects;
  using Core.Targeting;
  using Core.Zones;
  using Infrastructure;

  public class ViewModel : IReceive<UiInteractionChanged>
  {
    private readonly BindableCollection<Effect> _effects = new BindableCollection<Effect>();
    private readonly Game _game;
    private Action<Effect> _select = delegate { };

    public ViewModel(Game game)
    {
      _game = game;
      _game.Stack.EffectAdded += OnEffectAdded;
      _game.Stack.EffectRemoved += OnEffectRemoved;
    }

    public IEnumerable<Effect> Stack { get { return _effects; } }

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

    private void OnEffectRemoved(object sender, StackChangedEventArgs e)
    {
      _effects.Remove(e.Effect);
    }

    private void OnEffectAdded(object sender, StackChangedEventArgs e)
    {
      _effects.Add(e.Effect);
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