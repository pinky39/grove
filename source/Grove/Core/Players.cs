namespace Grove.Core
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public class Players : IEnumerable<Player>, IHashable
  {
    private readonly TrackableList<Player> _extraTurns;
    private Player _player1;
    private Player _player2;
    private Player _starting;

    public Players(
      string player1Name,
      ControllerType player1Type,
      IEnumerable<string> player1Deck,
      string player2Name,
      ControllerType player2Type,
      IEnumerable<string> player2Deck,
      Game game,
      bool enableDatabinding)
    {
      _extraTurns = new TrackableList<Player>(game.ChangeTracker, orderImpactsHashcode: true);

      Player1 = CreatePlayer(player1Name, "player1.png", player1Type, player1Deck, game, enableDatabinding);
      Player2 = CreatePlayer(player2Name, "player2.png", player2Type, player2Deck, game, enableDatabinding);
    }

    private Players() {}

    public Player Active { get { return Player1.IsActive ? Player1 : Player2; } }

    public bool AnotherMulliganRound
    {
      get
      {
        return
          (Player1.CanMulligan) ||
            (Player2.CanMulligan);
      }
    }

    public Player Attacking { get { return Active; } }
    public bool BothHaveLost { get { return Player1.HasLost && Player2.HasLost; } }
    public Player Computer { get { return Player1.IsHuman ? Player2 : Player1; } }
    public Player Defending { get { return Passive; } }
    public Player Human { get { return Player1.IsHuman ? Player1 : Player2; } }
    public Player this[int num] { get { return num == 0 ? Player1 : Player2; } }
    public Player Max { get { return Player1.IsMax ? Player1 : Player2; } }
    public Player Min { get { return GetOpponent(Max); } }
    public Player Passive { get { return GetOpponent(Active); } }

    public Player Player1
    {
      get { return _player1; }
      private set
      {
        _player1 = value;
        _player1.IsMax = true;
      }
    }

    public Player Player2
    {
      get { return _player2; }
      private set
      {
        _player2 = value;
        _player2.IsMax = false;
      }
    }

    public int Score { get { return Player1.Score + Player2.Score; } }

    public Player Starting
    {
      get { return _starting; }
      set
      {
        _starting = value;

        value.IsActive = true;
        GetOpponent(value).IsActive = false;
      }
    }

    public Player WithPriority { get { return _player1.HasPriority ? _player1 : _player2; } }

    public Player WithoutPriority { get { return _player1.HasPriority ? _player2 : _player1; } }

    public IEnumerator<Player> GetEnumerator()
    {
      yield return Player1;
      yield return Player2;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(Player1),
        calc.Calculate(Player2),
        calc.Calculate(_extraTurns));
    }

    private Player CreatePlayer(string name, string avatar, ControllerType type, IEnumerable<string> deck, Game game,
      bool enableDatabinding)
    {
      if (enableDatabinding)
      {
        return new Player(name, avatar, type, deck, game);
      }

      return new Player(name, avatar, type, deck, game);
    }

    public void SetAiVisibility(Player player)
    {
      Player1.SetAiVisibility(player);
      Player2.SetAiVisibility(player);
    }

    public void ChangeActivePlayer()
    {
      if (_extraTurns.Count > 0)
      {
        Player nextActive = _extraTurns.PopLast();
        Player opponent = GetOpponent(nextActive);

        nextActive.IsActive = true;
        opponent.IsActive = false;

        return;
      }

      Player1.IsActive = !Player1.IsActive;
      Player2.IsActive = !Player2.IsActive;
    }

    public Player GetOpponent(Player player)
    {
      return player == Player1 ? Player2 : Player1;
    }

    public void ScheduleExtraTurns(Player player, int count)
    {
      for (int i = 0; i < count; i++)
      {
        _extraTurns.Add(player);
      }
    }

    public void SetPriority(Player player)
    {
      player.HasPriority = true;
      GetOpponent(player).HasPriority = false;
    }

    public bool AnyHasLost()
    {
      return Player1.HasLost || Player2.HasLost;
    }

    public IEnumerable<Card> Permanents()
    {
      return this.SelectMany(x => x.Battlefield);
    }

    public void MoveDeadCreaturesToGraveyard()
    {
      foreach (Player player in this)
      {
        player.MoveCreaturesWithLeathalDamageOrZeroTougnessToGraveyard();
      }

      RespectLegendaryRule();
    }

    public void RemoveDamageFromPermanents()
    {
      foreach (Player player in this)
      {
        player.RemoveDamageFromPermanents();
      }
    }

    public void RemoveRegenerationFromPermanents()
    {
      foreach (Player player in this)
      {
        player.RemoveRegenerationFromPermanents();
      }
    }

    private void RespectLegendaryRule()
    {
      Card[] duplicateLegends = this
        .SelectMany(x => x.Battlefield.Legends)
        .GetDuplicates(card => card.Name).ToArray();

      foreach (Card legend in duplicateLegends)
      {
        legend.Sacrifice();
      }
    }

    public void DestroyPermanents(Func<Card, bool> filter = null, bool allowToRegenerate = true)
    {
      filter = filter ?? delegate { return true; };

      foreach (Card permanent in Permanents().ToList())
      {
        if (filter(permanent))
        {
          permanent.Destroy(allowToRegenerate);
        }
      }
    }

    public void ResetAiVisibility()
    {
      Player1.ResetAiVisibility();
      Player2.ResetAiVisibility();
    }
  }
}