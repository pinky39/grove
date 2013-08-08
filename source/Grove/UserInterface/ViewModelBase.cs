namespace Grove.UserInterface
{
  using System;
  using System.Windows;
  using Gameplay;
  using Gameplay.Tournaments;
  using Infrastructure;
  using Messages;
  using Persistance;
  using Shell;

  public abstract class ViewModelBase
  {
    public ViewModelFactories ViewModels { get; set; }
    public IShell Shell { get; set; }
    protected Game CurrentGame { get { return CurrentMatch.Game; } }
    public CardDatabase CardDatabase { get; set; }
    public MatchRunner MatchRunner { get; set; }
    protected Match CurrentMatch { get { return MatchRunner.Current; } }
    public TournamentRunner TournamentRunner { get; set; }
    protected Tournament CurrentTournament { get { return TournamentRunner.Current; } }
    protected Combat Combat { get { return CurrentGame.Combat; } }
    public CardFactory CardFactory { get; set; }


    private static readonly string[] ErrorMessages = new[]
      {
        "Errors have occurred.\nWe won't tell you where or why.\nLazy programmers.",
        "The code was willing\nIt considered your request,\nBut the chips were weak.",
        "Error reduces\nYour expensive computer\nTo a simple stone.",
        "To have no errors\nWould be life without meaning\nNo struggle, no joy",
        "Rather than a beep\nOr a rude error message,\nThese words: 'Game has crashed.'",
        "A file that big?\nIt might be very useful\nBut now it is gone.",
        "I just ate your data with some fava beans and a nice chianti."
      };

    protected Players Players { get { return CurrentGame.Players; } }

    public void ChangePlayersInterest(Card card)
    {
      Shell.Publish(new PlayersInterestChanged
        {
          Visual = card
        });
    }

    public void ChangePlayersInterest(CardViewModel card)
    {
      Shell.Publish(new PlayersInterestChanged
        {
          Visual = card
        });
    }

    protected void SaveGame()
    {
      if (CurrentMatch == null)
        return;

      var saveFileHeader = new SaveFileHeader();
      object gameData;

      if (CurrentMatch.IsTournament)
      {
        saveFileHeader.Description = CurrentTournament.Description;
        gameData = CurrentTournament.Save();
      }
      else
      {
        saveFileHeader.Description = string.Format("Single match, {0}", CurrentMatch.Description);
        gameData = CurrentMatch.Save();
      }

      SaveLoadHelper.WriteToDisk(saveFileHeader, gameData);
    }

    protected void HandleException(Exception ex)
    {
      LogFile.Error(ex.ToString());
      SaveGame();

      var message = ErrorMessages[RandomEx.Next(ErrorMessages.Length)];

      Shell.ShowMessageBox(message,
        MessageBoxButton.OK, DialogType.Large, title: "Enraged Monkey Error", icon: MessageBoxImage.Error);
    }

    public virtual void Initialize() {}
  }
}