namespace Grove.Ui.StartScreen
{
  using System.Windows;
  using Core;
  using Shell;

  public class ViewModel : IIsDialogHost
  {
    private readonly IShell _shell;
    private readonly CardDatabase _cardDatabase;
    private readonly DeckScreen.ViewModel.IFactory _deckScreenFactory;

    public ViewModel(IShell shell, CardDatabase cardDatabase, DeckScreen.ViewModel.IFactory deckScreenFactory)
    {
      _shell = shell;
      _cardDatabase = cardDatabase;
      _deckScreenFactory = deckScreenFactory;
    }

    public void AddDialog(object dialog, DialogType dialogType) {}

    public string DatabaseInfo
    {
      get { return string.Format("Database has {0} cards.", _cardDatabase.CardCount); }
    }
    
    public bool HasFocus(object dialog)
    {
      return false;
    }

    public void RemoveDialog(object dialog) {}

    public void Exit()
    {
      Application.Current.Shutdown();
    }

    public void Play()
    {
      var deckScreen = _deckScreenFactory.Create();
      _shell.ChangeScreen(deckScreen);
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}