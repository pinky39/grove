namespace Grove.UserInterface.LoadSavedGame
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class ViewModel : ViewModelBase
  {
    private readonly object _previousScreen;

    public ViewModel(object previousScreen)
    {
      _previousScreen = previousScreen;

      SavedGames = Grove.SavedGames.GetDescriptions().Select(info => new SavedGameViewModel
        {
          Filename = info.Name,
          Description = info.Description,
          LastSave = info.CreatedAt,
        })
        .OrderByDescending(x => x.LastSave)
        .ToList();

      if (SavedGames.Count == 0)
        return;

      Selected = SavedGames[0];
    }

    private IEnumerable<Func<string, bool>> Handlers
    {
      get
      {
        yield return (filename) =>
          {
            var file = Grove.SavedGames.Read(filename);
            var savedMatch = file.Data as SavedMatch;
            if (savedMatch == null) return false;

            Ui.Match = new Match(
              MatchParameters.Load(
                savedMatch,
                isTournament: false));

            Ui.Match.Start();
            return true;
          };

        yield return (filename) =>
          {
            var file = Grove.SavedGames.Read(filename);
            var savedTournament = file.Data as SavedTournament;
            if (savedTournament == null) return false;

            Ui.Tournament = new Tournament(
              TournamentParameters.Load(savedTournament));
            
            Ui.Tournament.Start();
            return true;
          };
      }
    }

    public SavedGameViewModel Selected { get; set; }
    public List<SavedGameViewModel> SavedGames { get; private set; }

    public bool CanLoad
    {
      get { return Selected != null; }
    }

    public void Load()
    {
      try
      {
        foreach (var loadGame in Handlers)
        {
          if (loadGame(Selected.Filename)) break;
        }
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }

      Shell.ChangeScreen(_previousScreen);
    }

    public void Back()
    {
      Shell.ChangeScreen(_previousScreen);
    }

    public interface IFactory
    {
      ViewModel Create(object previousScreen);
    }
  }
}