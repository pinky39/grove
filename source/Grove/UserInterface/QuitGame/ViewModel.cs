namespace Grove.UserInterface.QuitGame
{
  using System.Windows;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    public bool CanRematch { get { return !Match.IsTournament; } }

    public void QuitToMainMenu()
    {
      Match.Stop();
    }

    public void QuitToOperatingSystem()
    {
      Match.Stop();
      Application.Current.Shutdown();
    }

    public void Cancel()
    {
      this.Close();
    }

    public void Rematch()
    {
      Ui.Match.Rematch();
    }

    public void Save()
    {
      SaveGame();
      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}