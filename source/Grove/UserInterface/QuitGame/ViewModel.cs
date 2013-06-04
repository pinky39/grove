namespace Grove.UserInterface.QuitGame
{
  using System.IO;
  using System.Runtime.Serialization.Formatters.Binary;
  using System.Windows;
  using Infrastructure;
  using Persistance;

  public class ViewModel : ViewModelBase
  {
    public bool CanRematch { get { return !CurrentMatch.IsTournament; } }

    public void QuitToMainMenu()
    {
      CurrentMatch.Stop();
    }

    public void QuitToOperatingSystem()
    {
      CurrentMatch.Stop();
      Application.Current.Shutdown();
    }

    public void Cancel()
    {
      this.Close();
    }

    public void Rematch()
    {      
      MatchRunner.ForceRematch();
    }

    public void Save()
    {
      var formatter = new BinaryFormatter();
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

      using (var file = new FileStream(MediaLibrary.GetSaveGameFilename(), FileMode.Create))
      {
        formatter.Serialize(file, saveFileHeader);
        formatter.Serialize(file, gameData);
      }
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}