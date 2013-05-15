namespace Grove.Gameplay.Tournaments
{
  using System.Collections.Generic;

  public class TournamentPlayer
  {
    public TournamentPlayer(string name)
    {
      Name = name;
    }

    public string Name { get; private set; }
    public IEnumerable<string> Deck { get; set; }
  }
}