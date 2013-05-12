namespace Grove.UserInterface.QuitGame
{
  using System.Windows;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    public void QuitToMainMenu()
    {
      Match.ForceCurrentGameToEnd();
      var screen = ViewModels.StartScreen.Create();
      Shell.ChangeScreen(screen);
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