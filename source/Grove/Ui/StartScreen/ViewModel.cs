namespace Grove.Ui.StartScreen
{
  using System.Windows;
  using Core;

  public class ViewModel : IIsDialogHost
  {
    private readonly Match _match;
    private readonly CardDatabase _cardDatabase;

    public ViewModel(Match match, CardDatabase cardDatabase)
    {
      _match = match;
      _cardDatabase = cardDatabase;
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
      _match.Start();
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}