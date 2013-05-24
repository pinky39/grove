namespace Grove.Gameplay.Tournaments
{
  using System;

  public class TournamentPlayer : IComparable<TournamentPlayer>
  {
    public TournamentPlayer(string name, bool isHuman)
    {
      Name = name;
      IsHuman = isHuman;
    }

    public string Name { get; private set; }
    public bool IsHuman { get; private set; }
    public int WinCount { get; set; }
    public int DrawCount { get; set; }
    public int LooseCount { get; set; }
    public int GamesLost { get; set; }
    public int GamesWon { get; set; }
    public int GamesPlayed { get { return GamesWon + GamesLost; } }
    public int MatchesPlayed { get { return WinCount + LooseCount; } }
    public double GamesWonPercentage { get { return GamesPlayed == 0 ? 0 : (double) GamesWon*100/GamesPlayed; } }
    public int MatchPoints { get { return WinCount*3 + DrawCount; } }

    public Deck Deck { get; set; }

    public int CompareTo(TournamentPlayer other)
    {
      if (other.MatchPoints > MatchPoints)
        return 1;

      if (other.MatchPoints < MatchPoints)
        return -1;

      if (other.GamesWonPercentage > GamesWonPercentage)
        return 1;

      if (other.GamesWonPercentage < GamesWonPercentage)
        return -1;

      return Name.CompareTo(other.Name);
    }
  }
}