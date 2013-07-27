namespace Grove.Gameplay.Tournaments
{
  using System.Collections.Generic;
  using System.Linq;

  public class DraftResults
  {
    public DraftResults(IEnumerable<DraftPlayer> players)
    {
      Libraries = players.Select(x => x.Library).ToList();
    }

    public List<List<CardInfo>> Libraries { get; private set; }
  }
}