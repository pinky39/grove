namespace Grove.Persistance
{
  using System;
  using Gameplay.Tournaments;

  [Serializable]
  public class SavedTournament
  {
    public int RoundsToGo;
    public string PlayerName;    
    public TournamentPlayer[] Players;
    public SavedMatch SavedMatch;
    public bool HasMatchInProgress { get { return SavedMatch != null; } }
  }
}