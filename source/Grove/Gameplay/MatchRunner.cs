namespace Grove.Gameplay
{
  public class MatchRunner
  {
    private readonly Match.IFactory _matchFactory;
    private MatchParameters _matchParameters;

    public MatchRunner(Match.IFactory matchFactory)
    {
      _matchFactory = matchFactory;
    }

    public Match Current { get; private set; }

    public void Rematch()
    {
      Current.Stop();
      StartNew(_matchParameters);
    }

    public void StartNew(MatchParameters matchParameters)
    {
      var play = true;

      while (play)
      {
        Current = _matchFactory.Create(_matchParameters);
        Current.Run();

        play = Current.Rematch;
      }
    }

    public void StartNew(PlayerParameters player1, PlayerParameters player2, bool isTournament)
    {
      _matchParameters = new MatchParameters
        {
          Player1 = player1,
          Player2 = player2,
          IsTournament = isTournament
        };

      StartNew(_matchParameters);
    }
  }
}