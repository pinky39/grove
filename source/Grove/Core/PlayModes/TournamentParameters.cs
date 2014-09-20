namespace Grove
{
  public class TournamentParameters
  {
    public string PlayerName { get; private set; }
    public int AvatarId { get; private set; }
    public int PlayersCount { get; private set; }
    public string[] BoosterPacks { get; private set; }
    public string TournamentPack { get; private set; }
    public SavedTournament SavedTournament { get; private set; }
    public bool IsSavedTournament { get { return SavedTournament != null; } }

    public TournamentType Type { get; private set; }

    public static TournamentParameters Default(string playerName, int avatarId, int playersCount, string[] boosterPacks,
      string tournamentPack, TournamentType type)
    {
      return new TournamentParameters
        {
          PlayerName = playerName,
          PlayersCount = playersCount,
          BoosterPacks = boosterPacks,
          TournamentPack = tournamentPack,
          Type = type,
          AvatarId = avatarId
        };
    }

    public static TournamentParameters Load(SavedTournament savedTournament)
    {
      return new TournamentParameters
        {
          SavedTournament = savedTournament
        };
    }
  }
}