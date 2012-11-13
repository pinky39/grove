namespace Grove.Ui.CardInGraveyard
{
  using System;
  using Core;
  using Infrastructure;

  public class ViewModel : IReceive<TargetSelected>, IReceive<TargetUnselected>, IReceive<UiInteractionChanged>
  {
    private readonly Game _game;
    private Action _select = delegate { };

    public interface IFactory
    {
      ViewModel Create(Card card);
    }

    public ViewModel(Card card, Game game)
    {
      _game = game;
      Card = card;
    }

    public Card Card { get; private set; }
    public virtual bool IsSelected { get; protected set; }

    private void ChangeSelection()
    {
      _game.Publish(
        new SelectionChanged {Selection = Card});
    }

    public void Receive(TargetSelected message)
    {
      if (message.Target == Card)
      {
        IsSelected = true;
      }
    }

    public void Select()
    {
      _select();
    }

    public void Receive(TargetUnselected message)
    {
      if (message.Target == Card)
      {
        IsSelected = false;
      }
    }

    public void Receive(UiInteractionChanged message)
    {
        switch (message.State)
      {
        case (InteractionState.SelectTarget):
          {
            _select = ChangeSelection;
            break;
          }
        default:
          _select = delegate { };
          break;
      }
      
      IsSelected = false;
    }

    public void ChangePlayersInterest()
    {
      _game.Publish(new PlayersInterestChanged
        {
          Visual = Card
        });
    }
  }
}