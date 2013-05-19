namespace Grove.Gameplay.Tournaments
{
  public class TournamentPlayer
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
    public int MatchPoints { get { return WinCount*3 + DrawCount; } }

    public Deck Deck { get; set; }
  }
}