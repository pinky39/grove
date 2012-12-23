namespace Grove.Ui.SelectableCard
{
  using System;
  using Core;
  using Infrastructure;

  public class ViewModel : CardViewModel, IReceive<TargetSelected>, IReceive<TargetUnselected>,
    IReceive<UiInteractionChanged>
  {
    private readonly Game _game;
    private Action _select = delegate { };

    public ViewModel(Card card, Game game) : base(card)
    {
      _game = game;
    }

    public virtual bool IsSelected { get; protected set; }    

    public void Receive(TargetSelected message)
    {
      if (message.Target == Card)
      {
        IsSelected = true;
      }
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

    private void ChangeSelection()
    {
      _game.Publish(
        new SelectionChanged {Selection = Card});
    }

    public void Select()
    {
      _select();
    }

    public void ChangePlayersInterest()
    {
      _game.Publish(new PlayersInterestChanged
        {
          Visual = Card
        });
    }

    public interface IFactory
    {
      ViewModel Create(Card card);
      void Destroy(ViewModel viewModel);
    }
  }
}