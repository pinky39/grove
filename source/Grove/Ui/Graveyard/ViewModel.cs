namespace Grove.Ui.Graveyard
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Infrastructure;

  public class ViewModel : IReceive<SelectionModeChanged>
  {
    private readonly IPlayer _player;
    private readonly Publisher _publisher;
    private Action<Card> _select = delegate { };

    public ViewModel(Player player, Publisher publisher)
    {
      _player = player;
      _publisher = publisher;
    }

    public IEnumerable<Card> Cards { get { return _player.Graveyard; } }

    public void Receive(SelectionModeChanged message)
    {
      switch (message.SelectionMode)
      {
        case (SelectionMode.SelectTarget):
          {
            _select = MarkAsTarget;
            break;
          }
        default:
          _select = delegate { };
          break;
      }
    }

    public void Select(Card card)
    {
      _select(card);
    }

    private void MarkAsTarget(Card card)
    {
      _publisher.Publish(
        new TargetSelected {Target = card});
    }

    public void ChangePlayersInterest(Card card)
    {
      _publisher.Publish(new PlayersInterestChanged
        {
          Visual = card
        });
    }

    public interface IFactory
    {
      ViewModel Create(IPlayer player);
    }
  }
}