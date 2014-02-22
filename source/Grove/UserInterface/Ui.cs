namespace Grove.UserInterface
{
  using Gameplay;
  using Infrastructure;
  using Shell;

  public static class Ui
  {
    public static IShell Shell;
    public static Match Match;
    public static Tournament Tournament;
    public static Dialogs Dialogs;
    public static Configuration Configuration;
    public static Publisher Publisher = new Publisher().Initialize();    
  }
}