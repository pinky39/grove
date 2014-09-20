namespace Grove
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;

  public class Players : GameObject, IEnumerable<Player>, IHashable
  {
    private readonly TrackableList<Player> _extraTurns = new TrackableList<Player>(orderImpactsHashcode: true);
    private Player _starting;


    public Players(Player player1, Player player2)
    {
      Player1 = player1;
      Player1.IsMax = true;

      Player2 = player2;
      Player2.IsMax = false;
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

    public Player Searching { get; set; }
    public Player Attacking { get { return Active; } }
    public bool BothHaveLost { get { return Player1.HasLost && Player2.HasLost; } }
    public Player Computer { get { return Player1.IsHuman ? Player2 : Player1; } }
    public Player Defending { get { return Passive; } }
    public Player Human { get { return Player1.IsHuman ? Player1 : Player2; } }
    public Player this[int num] { get { return num == 0 ? Player1 : Player2; } }
    public Player Max { get { return Player1.IsMax ? Player1 : Player2; } }
    public Player Min { get { return GetOpponent(Max); } }
    public Player Passive { get { return GetOpponent(Active); } }

    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }

    public int Score { get { return Player1.Score + Player2.Score; } }

    public Player Starting
    {
      get { return _starting; }
      set
      {
        _starting = value;
        _starting.IsActive = true;
        _starting.Opponent.IsActive = false;
      }
    }

    public Player WithPriority { get { return Player1.HasPriority ? Player1 : Player2; } }
    public Player WithoutPriority { get { return Player1.HasPriority ? Player2 : Player1; } }

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
        calc.Calculate(Searching),   
        calc.Calculate(_extraTurns));
    }

    public void Initialize(Game game)
    {
      Game = game;

      _extraTurns.Initialize(ChangeTracker);
      Player1.Initialize(Game);
      Player2.Initialize(Game);
    }

    public void ChangeActivePlayer()
    {
      if (_extraTurns.Count > 0)
      {
        var nextActive = _extraTurns.PopLast();
        var opponent = GetOpponent(nextActive);

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
      for (var i = 0; i < count; i++)
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
      Active.MoveCreaturesWithLeathalDamageOrZeroTougnessToGraveyard();
      Passive.MoveCreaturesWithLeathalDamageOrZeroTougnessToGraveyard();                  
      RespectLegendaryRule();
    }

    public void RemoveDamageFromPermanents()
    {
      foreach (var player in this)
      {
        player.RemoveDamageFromPermanents();
      }
    }

    public void RemoveRegenerationFromPermanents()
    {
      foreach (var player in this)
      {
        player.RemoveRegenerationFromPermanents();
      }
    }

    private void RespectLegendaryRule()
    {
      var duplicateLegends = this
        .SelectMany(x => x.Battlefield.Legends)
        .GetDuplicates(card => card.Name).ToArray();

      foreach (var legend in duplicateLegends)
      {
        legend.Sacrifice();
      }
    }    
  }
}