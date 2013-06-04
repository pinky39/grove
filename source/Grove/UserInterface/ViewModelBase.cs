namespace Grove.UserInterface
{
  using Gameplay;
  using Gameplay.Tournaments;
  using Messages;
  using Shell;

  public abstract class ViewModelBase
  {
    public ViewModelFactories ViewModels { get; set; }
    public IShell Shell { get; set; }
    protected Game CurrentGame { get { return CurrentMatch.Game; } }
    public CardsInfo CardsInfo { get; set; }
    public MatchRunner MatchRunner { get; set; }
    protected Match CurrentMatch { get { return MatchRunner.Current; } }
    public TournamentRunner TournamentRunner { get; set; }
    protected Tournament CurrentTournament { get { return TournamentRunner.Current; } }
    protected Combat Combat {get { return CurrentGame.Combat; }}

    protected Players Players { get { return CurrentGame.Players; } }

    public void ChangePlayersInterest(Card card)
    {
      Shell.Publish(new PlayersInterestChanged
        {
          Visual = card
        });
    }

    public virtual void Initialize() {}
  }
}