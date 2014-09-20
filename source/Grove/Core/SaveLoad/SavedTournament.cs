namespace Grove
{
  using System;
  using System.Collections.Generic;

  [Serializable]
  public class SavedTournament
  {
    public List<TournamentMatch> CurrentRoundMatches;
    public int RoundsToGo;
    public SavedMatch SavedMatch;
    public bool HasMatchInProgress { get { return SavedMatch != null; } }
    public List<TournamentPlayer> Players { get; set; }
    public List<CardInfo> HumanLibrary { get; set; }
    public TournamentType Type { get; set; }
  }
}