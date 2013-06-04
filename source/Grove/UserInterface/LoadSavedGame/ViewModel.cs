namespace Grove.UserInterface.LoadSavedGame
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Runtime.Serialization.Formatters.Binary;
  using Gameplay;
  using Gameplay.Tournaments;
  using Persistance;

  public class ViewModel : ViewModelBase
  {
    private readonly object _previousScreen;

    public ViewModel(object previousScreen)
    {
      _previousScreen = previousScreen;
      var filenames = MediaLibrary.GetSavedGamesFilenames();

      if (filenames.Length == 0)
        return;

      SavedGames = filenames.Select((x, i) => new SavedGameViewModel
        {
          Filename = x,
          Description = GetDescription(x),
          LastSave = new FileInfo(x).LastWriteTime,
        })
        .OrderByDescending(x => x.LastSave)
        .ToList();

      Selected = SavedGames[0];
    }

    private IEnumerable<Func<object, bool>> Handlers
    {
      get
      {
        yield return (data) =>
          {
            var savedMatch = data as SavedMatch;
            if (savedMatch == null) return false;

            MatchRunner.Start(MatchParameters.Load(
              savedMatch, isTournament: false));

            return true;
          };

        yield return (data) =>
          {
            var savedTournament = data as SavedTournament;
            if (savedTournament == null) return false;

            TournamentRunner.Start(TournamentParameters.Load(savedTournament));
            return true;
          };
      }
    }

    public SavedGameViewModel Selected { get; set; }

    public List<SavedGameViewModel> SavedGames { get; private set; }

    public bool CanLoad { get { return Selected != null; } }

    private static string GetDescription(string filename)
    {
      var formatter = new BinaryFormatter();

      using (var file = new FileStream(filename, FileMode.Open))
      {
        var header = (SaveFileHeader) formatter.Deserialize(file);
        return header.Description;
      }
    }

    public void Load()
    {
      var formatter = new BinaryFormatter();

      object data;
      using (var file = new FileStream(Selected.Filename, FileMode.Open))
      {
        formatter.Deserialize(file);
        data = formatter.Deserialize(file);
      }

      foreach (var loadGame in Handlers)
      {
        if (loadGame(data)) break;
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