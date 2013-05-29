namespace Grove.Gameplay.Effects
{
  using System;
  using System.Linq;

  public class PutAllCardsFromGraveyardToBattlefield : Effect
  {
    private readonly bool _eachPlayer;
    private readonly Func<Card, bool> _filter;
    private readonly Action<Card> _modify;

    private PutAllCardsFromGraveyardToBattlefield() {}

    public PutAllCardsFromGraveyardToBattlefield(Func<Card, bool> filter, Action<Card> modify = null,
      bool eachPlayer = false)
    {
      _filter = filter;
      _modify = modify ?? delegate { };
      _eachPlayer = eachPlayer;
    }

    protected override void ResolveEffect()
    {
      if (_eachPlayer == false)
      {
        PutPlayersCardsToBattlefield(Controller);
        return;
      }

      PutPlayersCardsToBattlefield(Players.Active);
      PutPlayersCardsToBattlefield(Players.Passive);
    }

    private void PutPlayersCardsToBattlefield(Player player)
    {
      var ctrlCards = player.Graveyard.Where(_filter).ToList();

      foreach (var card in ctrlCards)
      {
        card.PutToBattlefield();
        _modify(card);
      }
    }
  }
}