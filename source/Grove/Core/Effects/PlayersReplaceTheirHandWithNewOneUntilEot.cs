namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Events;
  using Grove.Infrastructure;

  public class PlayersReplaceTheirHandWithNewOneUntilEot : Effect
  {
    protected override void ResolveEffect()
    {
      ReplacePlayersHandWithNewOneUntilEot(Players.Active);
      ReplacePlayersHandWithNewOneUntilEot(Players.Passive);
    }

    private void ReplacePlayersHandWithNewOneUntilEot(Player player)
    {
      var exiled = player.Hand.ToList();

      foreach (var card in exiled)
      {
        card.Exile();
      }

      player.DrawCards(7);
      Subscribe(new ReturnExiledCardsToHand(exiled, player, Game));
    }

    [Copyable]
    public class ReturnExiledCardsToHand : GameObject, IReceive<EndOfTurn>
    {
      private readonly Player _controller;
      private readonly List<Card> _exiledCards;

      private ReturnExiledCardsToHand() {}

      public ReturnExiledCardsToHand(List<Card> exiledCards, Player controller, Game game)
      {
        _controller = controller;
        Game = game;
        _exiledCards = exiledCards;
      }

      public void Receive(EndOfTurn message)
      {
        _controller.DiscardHand();

        foreach (var exiledCard in _exiledCards)
        {
          _controller.PutCardToHand(exiledCard);
        }

        Unsubscribe(this);
      }
    }
  }
}