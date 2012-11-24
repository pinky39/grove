namespace Grove.Ui.SelectableCard
{
  using System;
  using Core;
  using Core.Zones;
  using Infrastructure;

  public class ViewModel : IReceive<TargetSelected>, IReceive<TargetUnselected>, IReceive<UiInteractionChanged>
  {
    private readonly Game _game;
    private Action _select = delegate { };

    public ViewModel(Card card, Game game)
    {
      _game = game;
      Card = card;

      card.Property(x => x.IsRevealed)
        .Changes(this).Property<ViewModel, bool>(x => x.IsVisible);
    }

    public Card Card { get; private set; }
    public virtual bool IsSelected { get; protected set; }
    public bool IsVisible { get { return Card.Zone == Zone.Graveyard || Card.IsRevealed; } }

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
    }
  }
}