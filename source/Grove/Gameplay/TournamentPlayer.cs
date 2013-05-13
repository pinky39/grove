namespace Grove.Gameplay
{
  using System.Collections.Generic;

  public class TournamentPlayer
  {
    private readonly List<string> _library;

    public TournamentPlayer(string name, List<string> library)
    {
      _library = library;
      Name = name;
    }

    public string Name { get; private set; }
    public IEnumerable<string> Deck { get; set; }
    public IEnumerable<string> Library { get { return _library; } }
  }
}