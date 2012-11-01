namespace Grove.Ui.CardInGraveyard
{
  using System;
  using Core;
  using Infrastructure;

  public class ViewModel : IReceive<TargetSelected>, IReceive<TargetUnselected>, IReceive<UiInteractionChanged>
  {
    private readonly Publisher _publisher;
    private Action _select = delegate { };

    public interface IFactory
    {
      ViewModel Create(Card card);
    }

    public ViewModel(Card card, Publisher publisher)
    {
      Card = card;
      _publisher = publisher;
    }

    public Card Card { get; private set; }
    public virtual bool IsSelected { get; protected set; }

    private void ChangeSelection()
    {
      _publisher.Publish(
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
      _publisher.Publish(new PlayersInterestChanged
        {
          Visual = Card
        });
    }
  }
}