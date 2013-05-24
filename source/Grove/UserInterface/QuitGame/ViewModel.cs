namespace Grove.UserInterface.QuitGame
{
  using System.Windows;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    public void QuitToMainMenu()
    {
      Match.ForceCurrentGameToEnd();      
    }

    public void QuitToOperatingSystem()
    {
      Match.ForceCurrentGameToEnd();
      Application.Current.Shutdown();
    }

    public void Cancel()
    {
      this.Close();
    }    
    
    public bool CanRematch
    {
      get { return !Match.IsTournament; }
    }

    public void Rematch()
    {
      Match.ForceCurrentGameToEnd();
      Match.Rematch();
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}