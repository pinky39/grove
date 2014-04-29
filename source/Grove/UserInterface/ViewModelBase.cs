namespace Grove.UserInterface
{
  using System;
  using System.Windows;
  using Infrastructure;
  using Messages;
  using Shell;

  public abstract class ViewModelBase
  {
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

    public Dialogs ViewModels
    {
      get { return Ui.Dialogs; }
    }

    public Publisher Publisher
    {
      get { return Ui.Publisher; }
    }

    public IShell Shell
    {
      get { return Ui.Shell; }
    }

    protected Game Game
    {
      get { return Match.Game; }
    }

    protected Match Match
    {
      get { return Ui.Match; }
    }

    protected Tournament Tournament
    {
      get { return Ui.Tournament; }
    }

    protected Combat Combat
    {
      get { return Game.Combat; }
    }

    protected Players Players
    {
      get { return Game.Players; }
    }

    public void ChangePlayersInterest(object visual)
    {
      Ui.Publisher.Publish(new PlayersInterestChanged
      {
        Visual = visual
      });
    }
    
    public void ChangePlayersInterest(Card card)
    {
      Ui.Publisher.Publish(new PlayersInterestChanged
        {
          Visual = card
        });
    }

    public void ChangePlayersInterest(CardViewModel card)
    {
      Ui.Publisher.Publish(new PlayersInterestChanged
        {
          Visual = card
        });
    }

    protected void SaveGame()
    {
      if (Match == null)
        return;

      var saveFileHeader = new SaveFileHeader();
      object gameData;

      if (Match.IsTournament)
      {
        saveFileHeader.Description = Tournament.Description;
        gameData = Tournament.Save();
      }
      else
      {
        saveFileHeader.Description = string.Format("Single match, {0}", Match.Description);
        gameData = Match.Save();
      }

      SavedGames.Write(saveFileHeader, gameData);
    }

    protected void HandleException(Exception ex)
    {
      LogFile.Error(ex.ToString());
      SaveGame();

      var message = ErrorMessages[RandomEx.Next(ErrorMessages.Length)];

      Shell.ShowMessageBox(message,
        MessageBoxButton.OK, DialogType.Large, title: "Enraged Monkey Error", icon: MessageBoxImage.Error);
    }

    public virtual void Initialize()
    {
    }
  }
}