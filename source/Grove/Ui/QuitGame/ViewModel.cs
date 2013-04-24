namespace Grove.Ui.QuitGame
{
  using System.Windows;
  using Core;
  using Gameplay;
  using Infrastructure;
  using Shell;

  public class ViewModel
  {
    private readonly Match _match;
    private readonly IShell _shell;
    private readonly StartScreen.ViewModel.IFactory _startScreenFactory;

    public ViewModel(IShell shell, StartScreen.ViewModel.IFactory startScreenFactory, Match match)
    {
      _shell = shell;
      _startScreenFactory = startScreenFactory;
      _match = match;
    }

    public void QuitToMainMenu()
    {
      _match.ForceCurrentGameToEnd();
      var screen = _startScreenFactory.Create();
      _shell.ChangeScreen(screen);
    }

    public void QuitToOperatingSystem()
    {
      _match.ForceCurrentGameToEnd();
      Application.Current.Shutdown();
    }

    public void Cancel()
    {
      this.Close();
    }

    public void Rematch()
    {
      _match.ForceCurrentGameToEnd();
      _match.Rematch();
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}