namespace Grove.UserInterface.QuitGame
{
  using System.Windows;
  using Infrastructure;

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

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}