namespace Grove.Persistance
{
  using System;
  using System.Linq;
  using System.Collections.Generic;
  using Gameplay.Tournaments;

  [Serializable]
  public class SavedTournament
  {
    public int RoundsToGo;    
    public SavedMatch SavedMatch;    
    public List<TournamentMatch> CurrentRoundMatches;

    public bool HasMatchInProgress { get { return SavedMatch != null; } }
    public IEnumerable<TournamentPlayer> Players { get { return CurrentRoundMatches.SelectMany(x => new[] {x.Player1, x.Player2}); } }
  }
}