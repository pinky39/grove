namespace Grove.UserInterface.LoadTournament
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Infrastructure;
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

      SavedGames = filenames.Select((x,i) => new SavedGameViewModel
        {
          Filename = x,
          Description =  GetDescription(x),
          LastSave = new FileInfo(x).LastWriteTime,          
        })
        .OrderByDescending(x => x.LastSave)
        .ToList();

      Selected = SavedGames[0];
    }

    public SavedGameViewModel Selected { get; set; }

    private static string GetDescription(string filename)
    {
      var info = TournamentInfo.Load(filename);
      return String.Format("Sealed, {0} players, {1}", info.PlayerCount, info.Name);
    }

    public List<SavedGameViewModel> SavedGames { get; private set; }

    public bool CanLoad {get { return Selected != null; }}

    public void Load()
    {
      Tournament.Load(Selected.Filename);
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