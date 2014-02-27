namespace Grove
{
  public class MatchParameters
  {
    public PlayerParameters Player1 { get; private set; }
    public PlayerParameters Player2 { get; private set; }
    public bool IsTournament { get; private set; }
    public SavedMatch SavedMatch { get; private set; }
    public bool IsSavedMatch { get { return SavedMatch != null; } }

    public static MatchParameters Default(PlayerParameters player1, PlayerParameters player2, bool isTournament = false)
    {
      return new MatchParameters
        {
          Player1 = player1,
          Player2 = player2,
          IsTournament = isTournament
        };
    }

    public static MatchParameters Load(SavedMatch savedMatch, bool isTournament)
    {
      return new MatchParameters
        {
          Player1 = savedMatch.SavedGame.Player1,
          Player2 = savedMatch.SavedGame.Player2,
          IsTournament = isTournament,
          SavedMatch = savedMatch
        };
    }
  }
}