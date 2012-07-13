namespace Grove.Ui.StartScreen
{
  using System.Windows;
  using Core;
  using SelectDeck;
  using Shell;

  public class ViewModel : IIsDialogHost
  {
    private readonly CardDatabase _cardDatabase;
    private readonly SelectDeck.ViewModel.IFactory _selectDeckScreenFactory;
    private readonly IShell _shell;


    public ViewModel(IShell shell, CardDatabase cardDatabase, SelectDeck.ViewModel.IFactory selectDeckScreenFactory)
    {
      _shell = shell;
      _cardDatabase = cardDatabase;
      _selectDeckScreenFactory = selectDeckScreenFactory;
    }

    public string DatabaseInfo { get { return string.Format("Database has {0} cards.", _cardDatabase.CardCount); } }
    public void AddDialog(object dialog, DialogType dialogType) {}

    public bool HasFocus(object dialog)
    {
      return false;
    }

    public void CloseAllDialogs() {}

    public void RemoveDialog(object dialog) {}

    public void Exit()
    {
      Application.Current.Shutdown();
    }

    public void Play()
    {
      var deckScreen = _selectDeckScreenFactory.Create(ScreenType.YourDeck, this);
      _shell.ChangeScreen(deckScreen);
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}