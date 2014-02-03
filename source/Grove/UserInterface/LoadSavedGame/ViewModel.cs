namespace Grove.UserInterface.LoadSavedGame
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Tournaments;
  using Persistance;

  public class ViewModel : ViewModelBase
  {
    private readonly object _previousScreen;

    public ViewModel(object previousScreen)
    {
      _previousScreen = previousScreen;

      SavedGames = Persistance.SavedGames.GetDescriptions().Select(info => new SavedGameViewModel
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
            var file = Persistance.SavedGames.Read(filename);
            var savedMatch = file.Data as SavedMatch;
            if (savedMatch == null) return false;

            MatchRunner.Start(MatchParameters.Load(
              savedMatch, isTournament: false));

            return true;
          };

        yield return (filename) =>
          {
            var file = Persistance.SavedGames.Read(filename);
            var savedTournament = file.Data as SavedTournament;
            if (savedTournament == null) return false;

            TournamentRunner.Start(TournamentParameters.Load(savedTournament));
            return true;
          };
      }
    }

    public SavedGameViewModel Selected { get; set; }
    public List<SavedGameViewModel> SavedGames { get; private set; }

    public bool CanLoad { get { return Selected != null; } }

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